using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.ItensCardapio
{
    public class CreateModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public CreateModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ItemCardapio ItemCardapio { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ItensCardapio.Add(ItemCardapio);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
