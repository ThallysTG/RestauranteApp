using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Pages.Pedidos
{
    public class IndexModel : PageModel
    {
        private readonly RestauranteApp.Data.ApplicationDbContext _context;
        public IndexModel(RestauranteApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public async Task OnGetAsync()
        {
            Pedidos = await _context.Pedidos
                .Include(p => p.Atendimento)
                .OrderByDescending(p => p.Data)
                .ToListAsync();
        }
    }
}
