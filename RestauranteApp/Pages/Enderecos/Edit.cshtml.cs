using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

namespace RestauranteApp.Pages.Enderecos
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(RestauranteApp.Data.ApplicationDbContext context,
                         UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public EnderecoEntrega EnderecoEntrega { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await EnsurePerfilAsync();

            EnderecoEntrega = await _context.EnderecosEntrega
                .FirstOrDefaultAsync(e => e.Id == id && e.PerfilClienteId == perfil.Id);

            if (EnderecoEntrega == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("EnderecoEntrega.Perfil");
            ModelState.Remove("EnderecoEntrega.PerfilClienteId");
            if (!ModelState.IsValid) return Page();

            var perfil = await EnsurePerfilAsync();

            // Confirma que é do usuário
            var original = await _context.EnderecosEntrega
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == EnderecoEntrega.Id && e.PerfilClienteId == perfil.Id);
            if (original == null) return NotFound();

            EnderecoEntrega.PerfilClienteId = perfil.Id;
            _context.Attach(EnderecoEntrega).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
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
