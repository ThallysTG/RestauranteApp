using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteApp.Pages.Ingredientes
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
        public Ingrediente Ingrediente { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (await _context.Ingredientes.AnyAsync(i => i.Nome == Ingrediente.Nome))
            {
                ModelState.AddModelError("Ingrediente.Nome", "Já existe um ingrediente com esse nome.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Ingredientes.Add(Ingrediente);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
