using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteApp.Pages.Enderecos
{
    public class IndexModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(RestauranteApp.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<EnderecoEntrega> Enderecos { get; set; } = new List<EnderecoEntrega>();

        public async Task OnGetAsync()
        {
            var perfil = await EnsurePerfilAsync();
            Enderecos = await _context.EnderecosEntrega
                .Where(e => e.PerfilClienteId == perfil.Id)
                .OrderBy(e => e.Titulo)
                .ToListAsync();
        }

        private async Task<PerfilCliente> EnsurePerfilAsync()
        {
            var userId = _userManager.GetUserId(User)!;
            var perfil = await _context.PerfisClientes.FirstOrDefaultAsync(p => p.UsuarioId == userId);
            if (perfil is null)
            {
                perfil = new PerfilCliente { UsuarioId = userId };
                _context.PerfisClientes.Add(perfil);
                await _context.SaveChangesAsync();
            }
            return perfil;
        }
    }
}
