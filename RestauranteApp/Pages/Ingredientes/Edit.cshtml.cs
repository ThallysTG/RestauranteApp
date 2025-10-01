using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.Ingredientes
{
    public class EditModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public EditModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Ingrediente Ingrediente { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente =  await _context.Ingredientes.FirstOrDefaultAsync(m => m.Id == id);
            if (ingrediente == null)
            {
                return NotFound();
            }
            Ingrediente = ingrediente;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Ingrediente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredienteExists(Ingrediente.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IngredienteExists(int id)
        {
            return _context.Ingredientes.Any(e => e.Id == id);
        }
    }
}
