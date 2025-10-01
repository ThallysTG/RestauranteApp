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
    public class DeleteModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public DeleteModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemcardapio = await _context.ItensCardapio.FindAsync(id);
            if (itemcardapio != null)
            {
                ItemCardapio = itemcardapio;
                _context.ItensCardapio.Remove(ItemCardapio);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
