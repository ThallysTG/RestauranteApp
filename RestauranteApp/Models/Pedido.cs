using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        public Periodo PeriodoPedido { get; set; } // almoco/jantar

        public string UsuarioId { get; set; } = string.Empty;

        // 1–1 com Atendimento
        public int AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; } = default!;

        // n–n via PedidoItem
        public ICollection<PedidoItem> Itens { get; set; } = new List<PedidoItem>();

        // totais calculados para relatorios dps
        public decimal SubtotalItens { get; set; }
        public decimal TotalDescontos { get; set; }
        public decimal TotalTaxas { get; set; }
        public decimal TotalFinal { get; set; }

        public int? EnderecoEntregaId { get; set; }          // null para Presencial
        public EnderecoEntrega? EnderecoEntrega { get; set; }
    }
}