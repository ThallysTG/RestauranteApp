using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.Mesas
{
    public class DetailsModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public DetailsModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Mesa Mesa { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);

            if (mesa is not null)
            {
                Mesa = mesa;

                return Page();
            }

            return NotFound();
        }
    }
}
