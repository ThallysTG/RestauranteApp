using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Models;

namespace RestauranteApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ItemCardapio> ItensCardapio => Set<ItemCardapio>();
        public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
        public DbSet<ItemCardapioIngrediente> ItensCardapioIngredientes => Set<ItemCardapioIngrediente>();
        public DbSet<SugestaoDoChefe> SugestoesDoChefe => Set<SugestaoDoChefe>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<Mesa> Mesas => Set<Mesa>();
        public DbSet<Reserva> Reservas => Set<Reserva>();
        public DbSet<PerfilCliente> PerfisClientes => Set<PerfilCliente>();



        protected override void OnModelCreating(ModelBuilder b) { base.OnModelCreating(b); }
    }
}
