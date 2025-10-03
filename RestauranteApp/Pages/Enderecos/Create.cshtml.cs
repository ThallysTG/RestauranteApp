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
    public class CreateModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(RestauranteApp.Data.ApplicationDbContext context,
                           UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public EnderecoEntrega EnderecoEntrega { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // (não vem do form)
            ModelState.Remove(nameof(EnderecoEntrega.PerfilClienteId));
            ModelState.Remove("EnderecoEntrega.PerfilCliente.Id"); 

            // resolve ou cria o Perfil do usuário logado
            var perfil = await EnsurePerfilAsync();

            // liga o endereço ao perfil do usuário
            EnderecoEntrega.PerfilClienteId = perfil.Id;

            if (!ModelState.IsValid)
                return Page();

            _context.EnderecosEntrega.Add(EnderecoEntrega);
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
