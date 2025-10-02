using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.Reservas
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(RestauranteApp.Data.ApplicationDbContext context, UserManager<IdentityUser> _userManager)
        {
            _context = context;
            _userManager = _userManager;
        }

        public IList<Reserva> Lista { get;set; } = new List<Reserva>();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            Lista = await _context.Reservas
                .Include(r => r.Mesa)
                .Where(r => r.UsuarioId == userId && r.DataHora >= DateTime.Today)
                .OrderBy(r => r.DataHora)
                .ToListAsync();
        }
    }
}
