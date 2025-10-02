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
    public class CreateModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;

        public CreateModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public SugestaoDoChefe Sugestao { get; set; } = new SugestaoDoChefe
        {
            Data = DateOnly.FromDateTime(DateTime.Today),
            Periodo = Periodo.Almoco
        };

        // select de itens filtrado por período
        public SelectList ItensOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await CarregarItensAsync(Sugestao.Periodo);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CarregarItensAsync(Sugestao.Periodo);

            if (!ModelState.IsValid)
                return Page();

            // 1 sugestão por Data + Período
            var jaExiste = await _context.SugestoesDoChefe
                .AnyAsync(s => s.Data == Sugestao.Data && s.Periodo == Sugestao.Periodo);

            if (jaExiste)
            {
                ModelState.AddModelError(string.Empty, "Já existe uma sugestão para essa data e período.");
                return Page();
            }

            _context.SugestoesDoChefe.Add(Sugestao);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Conflito ao salvar.");
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