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
    public class DeleteModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public DeleteModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mesa Mesa { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            // carrega a mesa já com as reservas para exibir um aviso se houver
            Mesa = await _context.Mesas
                .Include(m => m.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Mesa == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            // carrega novamente com reservas para checar no POST
            var mesa = await _context.Mesas
                .Include(m => m.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mesa == null) return NotFound();


            // regra: não permitir exclusão se houver reservas
            if (mesa.Reservas != null && mesa.Reservas.Any())
            {
                // reatribui para reexibir a página com a mensagem de erro
                Mesa = mesa;
                ModelState.AddModelError(string.Empty, "Não é possível excluir uma mesa que possui reservas.");
                return Page();
            }

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}