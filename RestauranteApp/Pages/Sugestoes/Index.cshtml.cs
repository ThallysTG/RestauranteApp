using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class IndexModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        public IndexModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SugestaoDoChefe> Lista { get; set; } = new List<SugestaoDoChefe>();

        public DateOnly InicioSemana { get; set; }
        public DateOnly FimSemana { get; set; }

        public async Task OnGetAsync()
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            int delta = (int)hoje.DayOfWeek; // domingo=0
            InicioSemana = hoje.AddDays(-delta);
            FimSemana = InicioSemana.AddDays(6);

            Lista = await _context.SugestoesDoChefe
                .Include(s => s.Item)
                .Where(s => s.Data >= InicioSemana && s.Data <= FimSemana)
                .OrderBy(s => s.Data).ThenBy(s => s.Periodo)
                .ToListAsync();
        }
    }
}