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
    public class DetailsModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public DetailsModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ItemCardapio Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Item = await _context.ItensCardapio
                .Include(i => i.Ingredientes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Item == null) return NotFound();

            return Page();
        }
    }
}
