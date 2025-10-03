using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.Pedidos
{
    public class DetailsModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public DetailsModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Pedido Pedido { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Atendimento)
                .Include(p => p.Itens)
                    .ThenInclude(pi => pi.Item)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido is null) return NotFound();
            Pedido = pedido;
            return Page();
        }
    }
}

