using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestauranteApp.Data;
using RestauranteApp.Models;
using RestauranteApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RestauranteApp.Pages.Pedidos
{
    public class CreateModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PedidoPricingService _pricing;

        public CreateModel(RestauranteApp.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager, PedidoPricingService pricing)
        {
            _context = context;
            _userManager = userManager;
            _pricing = pricing;
        }
        // ViewModel simples para montar o pedido
        public class LinhaItemVM
        {
            public int ItemCardapioId { get; set; }
            public string Nome { get; set; } = "";
            public decimal Preco { get; set; }
            [Range(0, 999)]
            public int Quantidade { get; set; } 
            public Periodo Periodo { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public Periodo PeriodoPedido { get; set; } = Periodo.Almoco;

        [BindProperty]
        public string TipoAtendimento { get; set; } = "Presencial";

        // campos específicos dos atendimentos
        [BindProperty] public decimal? TaxaFixa { get; set; }           // delivery próprio/app
        [BindProperty] public decimal? ComissaoPercentual { get; set; } // app
        [BindProperty] public string? Parceiro { get; set; }            // app

        [BindProperty]
        public List<LinhaItemVM> Itens { get; set; } = new();

        public async Task OnGetAsync()
        {
            await CarregarItensAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            var itensSelecionados = Itens
                .Where(i => i.Quantidade > 0)
                .Select(i => new PedidoItem
                {
                    ItemCardapioId = i.ItemCardapioId,
                    Quantidade = i.Quantidade
                })
                .ToList();

            if (!itensSelecionados.Any())
            {
                ModelState.AddModelError(string.Empty, "Selecione pelo menos um item do período.");
                await CarregarItensAsync();  
                return Page();
            }

            Atendimento atendimento = TipoAtendimento switch
            {
                "Presencial" => new AtendimentoPresencial(),
                "DeliveryProprio" => new AtendimentoDeliveryProprio { TaxaFixa = TaxaFixa ?? 0m },
                "DeliveryAplicativo" => new AtendimentoDeliveryAplicativo
                {
                    Parceiro = Parceiro ?? "",
                    TaxaFixa = TaxaFixa,
                    ComissaoPercentual = ComissaoPercentual ?? 0m
                },
                _ => new AtendimentoPresencial()
            };

            var pedido = new Pedido
            {
                UsuarioId = _userManager.GetUserId(User)!,
                Data = DateTime.Now,
                PeriodoPedido = PeriodoPedido,
                Atendimento = atendimento,
                Itens = itensSelecionados
            };

            await _pricing.CalcularTotaisAsync(pedido);   
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = pedido.Id });
        }

        private async Task CarregarItensAsync()
        {
            Itens = await _context.ItensCardapio
                .Where(i => i.Periodo == PeriodoPedido) // filtro por período
                .OrderBy(i => i.Nome)
                .Select(i => new LinhaItemVM
                {
                    ItemCardapioId = i.Id,
                    Nome = i.Nome,
                    Preco = i.PrecoBase,
                    Quantidade = 0,
                    Periodo = i.Periodo
                })
                .ToListAsync();
        }

    }
}