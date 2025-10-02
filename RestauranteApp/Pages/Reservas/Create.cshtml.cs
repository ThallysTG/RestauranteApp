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
    public class CreateModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(RestauranteApp.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public Reserva Reserva { get; set; } = new Reserva();

        public SelectList MesasOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await CarregarMesasAsync();
            // sugestão: pré-setar para hoje às 19:00
            Reserva.DataHora = DateTime.Today.AddHours(19);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CarregarMesasAsync();

            // vincula usuário logado
            Reserva.UsuarioId = _userManager.GetUserId(User);

            // horario do jantar (19h–22h)
            if (!DentroJanelaJantar(Reserva.DataHora))
            {
                ModelState.AddModelError("Reserva.DataHora", "Reservas apenas entre 19:00 e 22:00.");
                return Page();
            }

            // off ter outra reserva na mesma mesa, mesmo instante
            var conflito = await _context.Reservas.AnyAsync(r =>
                r.MesaId == Reserva.MesaId &&
                r.DataHora == Reserva.DataHora);

            if (conflito)
            {
                ModelState.AddModelError(string.Empty, "Já existe uma reserva para essa mesa nesse horário.");
                return Page();
            }

            _context.Reservas.Add(Reserva);
            await _context.SaveChangesAsync();
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
