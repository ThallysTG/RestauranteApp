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
    public class IndexModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public IndexModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Mesa> Mesa { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Mesa = await _context.Mesas.ToListAsync();
        }
    }
}
