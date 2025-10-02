using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.ItensCardapio
{
    public class IndexModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public IndexModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ItemCardapio> Itens { get;set; } = new List<ItemCardapio>();


        public async Task OnGetAsync()
        {
            Itens = await _context.ItensCardapio
                .Include(i => i.Ingredientes)
                .ToListAsync();
        }
    }
}
