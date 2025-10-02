using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.ItensCardapio
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ItemCardapio Item { get; set; } = default!;

        [BindProperty]
        public List<int> IngredientesSelecionados { get; set; } = new List<int>();

        public List<Ingrediente> TodosIngredientes { get; set; } = new List<Ingrediente>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Item = await _context.ItensCardapio
                .Include(i => i.Ingredientes)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (Item == null) return NotFound();

            TodosIngredientes = await _context.Ingredientes
                .OrderBy(i => i.Nome)
                .ToListAsync();

            IngredientesSelecionados = Item.Ingredientes.Select(i => i.Id).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var itemDb = await _context.ItensCardapio
                .Include(i => i.Ingredientes)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (itemDb == null) return NotFound();

            if (!ModelState.IsValid)
            {
                Item = itemDb;
                TodosIngredientes = await _context.Ingredientes
                    .OrderBy(i => i.Nome)
                    .ToListAsync();
                IngredientesSelecionados ??= new List<int>();
                return Page();
            }

            // Atualiza campos simples
            itemDb.Nome = Item.Nome;
            itemDb.Descricao = Item.Descricao;
            itemDb.PrecoBase = Item.PrecoBase;
            itemDb.Periodo = Item.Periodo;

            // --- Sincroniza a relação N–N ---
            var selecionados = (IngredientesSelecionados ?? new List<int>()).ToHashSet();

            // remove os que saíram
            var remover = itemDb.Ingredientes.Where(i => !selecionados.Contains(i.Id)).ToList();
            foreach (var r in remover)
                itemDb.Ingredientes.Remove(r);

            // adiciona os novos
            var atuais = itemDb.Ingredientes.Select(i => i.Id).ToHashSet();
            var novos = await _context.Ingredientes
                .Where(i => selecionados.Contains(i.Id) && !atuais.Contains(i.Id))
                .ToListAsync();
            foreach (var n in novos)
                itemDb.Ingredientes.Add(n);

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
