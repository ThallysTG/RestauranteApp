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

namespace RestauranteApp.Pages.Mesas
{
    public class EditModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public EditModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mesa Mesa { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa =  await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);
            if (mesa == null)
            {
                return NotFound();
            }
            Mesa = mesa;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (await _context.Mesas.AnyAsync(m => m.Numero == Mesa.Numero && m.Id != Mesa.Id))
            {
                ModelState.AddModelError("Mesa.Numero", "Já existe uma mesa com este número.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Mesa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MesaExists(Mesa.Id))
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

        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.Id == id);
        }
    }
}
