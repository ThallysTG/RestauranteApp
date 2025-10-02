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

namespace RestauranteApp.Pages.Reservas
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(RestauranteApp.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager    )
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        public SelectList MesasOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Reserva = await _context.Reservas
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Reserva == null) return NotFound();

            await CarregarMesasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CarregarMesasAsync();

            if (!ModelState.IsValid) return Page();

            if (!DentroJanelaJantar(Reserva.DataHora))
            {
                ModelState.AddModelError("Reserva.DataHora", "Reservas apenas entre 19:00 e 22:00.");
                return Page();
            }

            Reserva.UsuarioId = _userManager.GetUserId(User); // mantém usuário


            // outra reserva com mesma mesa/hora e ID diferente
            var conflito = await _context.Reservas.AnyAsync(r =>
                r.MesaId == Reserva.MesaId &&
                r.DataHora == Reserva.DataHora &&
                r.Id != Reserva.Id);

            if (conflito)
            {
                ModelState.AddModelError(string.Empty, "Já existe uma reserva para essa mesa nesse horário.");
                return Page();
            }

            // atualiza
            _context.Attach(Reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // caso o índice único dispare por alguma condição de corrida
                ModelState.AddModelError(string.Empty, "Conflito de reserva (índice único). Tente outro horário.");
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private bool DentroJanelaJantar(DateTime dt)
        {
            var hora = dt.TimeOfDay;
            var inicio = new TimeSpan(19, 0, 0);
            var fimExclusivo = new TimeSpan(22, 0, 0);
            return hora >= inicio && hora < fimExclusivo;
        }

        private async Task CarregarMesasAsync()
        {
            var mesas = await _context.Mesas.OrderBy(m => m.Numero).ToListAsync();
            MesasOptions = new SelectList(mesas, "Id", "Numero");
        }
    }
}