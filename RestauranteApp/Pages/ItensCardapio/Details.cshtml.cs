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

        public ItemCardapio ItemCardapio { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemcardapio = await _context.ItensCardapio.FirstOrDefaultAsync(m => m.Id == id);

            if (itemcardapio is not null)
            {
                ItemCardapio = itemcardapio;

                return Page();
            }

            return NotFound();
        }
    }
}
