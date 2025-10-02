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

namespace RestauranteApp.Pages.ItensCardapio
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ItemCardapio Item { get; set; } = new ItemCardapio();

        // IDs marcados no formulário
        [BindProperty]
        public List<int> IngredientesSelecionados { get; set; } = new List<int>();

        // Lista para renderizar os checkboxes
        public List<Ingrediente> TodosIngredientes { get; set; } = new List<Ingrediente>();

        public async Task<IActionResult> OnGetAsync()
        {
            TodosIngredientes = await _context.Ingredientes
                .OrderBy(i => i.Nome)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TodosIngredientes = await _context.Ingredientes
                    .OrderBy(i => i.Nome)
                    .ToListAsync();
                return Page();
            }

            // Anexa os ingredientes marcados (N–N automático)
            if (IngredientesSelecionados != null && IngredientesSelecionados.Count > 0)
            {
                var ingrs = await _context.Ingredientes
                    .Where(i => IngredientesSelecionados.Contains(i.Id))
                    .ToListAsync();

                foreach (var ing in ingrs)
                    Item.Ingredientes.Add(ing);
            }

            _context.ItensCardapio.Add(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}