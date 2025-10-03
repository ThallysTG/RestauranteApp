using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Services
{
    public class PedidoPricingService
    {
        private readonly ApplicationDbContext _ctx;
        public PedidoPricingService(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task CalcularTotaisAsync(Pedido pedido)
        {
            // carrega sugestão do dia/período
            var data = DateOnly.FromDateTime(pedido.Data);
            var sugestao = await _ctx.SugestoesDoChefe
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Data == data && s.Periodo == pedido.PeriodoPedido);

            decimal subtotal = 0m, descontos = 0m;

            foreach (var it in pedido.Itens)
            {
                // garante preço base do item actual
                if (it.Item == null)
                    it.Item = await _ctx.ItensCardapio.FindAsync(it.ItemCardapioId) ?? throw new InvalidOperationException("Item inválido");

                it.PrecoUnitarioBase = it.Item.PrecoBase;

                // regra período: almoço só itens de almoço; jantar só itens de jantar
                if (it.Item.Periodo != pedido.PeriodoPedido)
                    throw new InvalidOperationException("Item fora do período do pedido.");

                // desconto 20% se for sugestão do chefe do dia/período
                var aplicaDesconto = (sugestao != null && sugestao.ItemCardapioId == it.ItemCardapioId);
                it.DescontoUnitarioAplicado = aplicaDesconto ? Math.Round(it.PrecoUnitarioBase * 0.20m, 2) : 0m;

                subtotal += it.PrecoUnitarioBase * it.Quantidade;
                descontos += it.DescontoUnitarioAplicado * it.Quantidade;
            }

            // taxa por atendimento
            decimal taxas = pedido.Atendimento switch
            {
                AtendimentoPresencial => 0m,
                AtendimentoDeliveryProprio dp => dp.TaxaFixa,
                AtendimentoDeliveryAplicativo da =>
                    (da.TaxaFixa ?? 0m) + Math.Round(subtotal * (da.ComissaoPercentual / 100m), 2),
                _ => 0m
            };

            pedido.SubtotalItens = subtotal;
            pedido.TotalDescontos = descontos;
            pedido.TotalTaxas = taxas;
            pedido.TotalFinal = subtotal - descontos + taxas;
        }
    }
}