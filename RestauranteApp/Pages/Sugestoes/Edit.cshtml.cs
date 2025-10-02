using Microsoft.AspNetCore.Authorization;
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

namespace RestauranteApp.Pages.Sugestoes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public EditModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SugestaoDoChefe Sugestao { get; set; } = default!;

        public SelectList ItensOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Sugestao = await _context.SugestoesDoChefe
                .Include(s => s.Item)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Sugestao == null) return NotFound();

            await CarregarItensAsync(Sugestao.Periodo);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CarregarItensAsync(Sugestao.Periodo);

            if (!ModelState.IsValid) return Page();

            // apenas 1 sugestao
            var conflito = await _context.SugestoesDoChefe
                .AnyAsync(s => s.Id != Sugestao.Id &&
                               s.Data == Sugestao.Data &&
                               s.Periodo == Sugestao.Periodo);

            if (conflito)
            {
                ModelState.AddModelError(string.Empty, "Já existe uma sugestão para essa data e período.");
                return Page();
            }

            // Atualiza
            _context.Attach(Sugestao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Conflito ao salvar (índice único Data+Período).");
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private async Task CarregarItensAsync(Periodo periodo)
        {
            var itens = await _context.ItensCardapio
                .Where(i => i.Periodo == periodo)
                .OrderBy(i => i.Nome)
                .ToListAsync();

            ItensOptions = new SelectList(itens, "Id", "Nome");
        }
    }
}