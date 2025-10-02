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

namespace RestauranteApp.Pages.Mesas
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
        public Mesa Mesa { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (await _context.Mesas.AnyAsync(m => m.Numero == Mesa.Numero))
            {
                ModelState.AddModelError("Mesa.Numero", "Já existe uma mesa com este número.");
                return Page();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Mesas.Add(Mesa);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
