using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;
using RestauranteApp.Models.ViewModels;
using System.Diagnostics;

namespace RestauranteApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public HomeController(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<IActionResult> Index()
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);

            var almoco = await _ctx.SugestoesDoChefe
                .Include(s => s.Item)
                .Where(s => s.Data == hoje && s.Periodo == Periodo.Almoco)
                .Select(s => s.Item)
                .FirstOrDefaultAsync();

            var jantar = await _ctx.SugestoesDoChefe
                .Include(s => s.Item)
                .Where(s => s.Data == hoje && s.Periodo == Periodo.Jantar)
                .Select(s => s.Item)
                .FirstOrDefaultAsync();

            var vm = new HomeSugestoesVM
            {
                SugestaoAlmoco = almoco,
                SugestaoJantar = jantar
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();
    }
}
