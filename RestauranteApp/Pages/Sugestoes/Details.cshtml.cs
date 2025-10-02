using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.Sugestoes
{
    public class DetailsModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public DetailsModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public SugestaoDoChefe SugestaoDoChefe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sugestaodochefe = await _context.SugestoesDoChefe.FirstOrDefaultAsync(m => m.Id == id);

            if (sugestaodochefe is not null)
            {
                SugestaoDoChefe = sugestaodochefe;

                return Page();
            }

            return NotFound();
        }
    }
}
