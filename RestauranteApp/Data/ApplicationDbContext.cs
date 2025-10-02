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
        //public DbSet<ItemCardapioIngrediente> ItensCardapioIngredientes => Set<ItemCardapioIngrediente>();
        public DbSet<SugestaoDoChefe> SugestoesDoChefe => Set<SugestaoDoChefe>();
        //public DbSet<Pedido> Pedidos => Set<Pedido>();
        //public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();

        public DbSet<Mesa> Mesas => Set<Mesa>();
        public DbSet<Reserva> Reservas => Set<Reserva>();
        //public DbSet<PerfilCliente> PerfisClientes => Set<PerfilCliente>();

        //public DbSet<Atendimento> Atendimentos => Set<Atendimento>();
        //public DbSet<EnderecoEntrega> EnderecosEntrega => Set<EnderecoEntrega>();
       //public DbSet<ParceiroApp> ParceirosApp => Set<ParceiroApp>();

        protected override void OnModelCreating(ModelBuilder b) { 
            base.OnModelCreating(b);

            b.Entity<ItemCardapio>()
                .HasMany(i => i.Ingredientes)
                .WithMany(i => i.Itens);

            b.Entity<Reserva>()
                .HasIndex(r => new { r.MesaId, r.DataHora })
                .IsUnique();

            b.Entity<Mesa>()
                .HasIndex(m => m.Numero)
                .IsUnique();

            b.Entity<SugestaoDoChefe>()
                .HasIndex(s => new { s.Data, s.Periodo })
                .IsUnique(); // 1 sugestão por dia e por período
        }
}
}
