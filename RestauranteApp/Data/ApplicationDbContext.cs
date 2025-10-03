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
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();

        public DbSet<Mesa> Mesas => Set<Mesa>();
        public DbSet<Reserva> Reservas => Set<Reserva>();
        public DbSet<PerfilCliente> PerfisClientes => Set<PerfilCliente>();

        public DbSet<Atendimento> Atendimentos => Set<Atendimento>();
        public DbSet<EnderecoEntrega> EnderecosEntrega => Set<EnderecoEntrega>();
       //public DbSet<ParceiroApp> ParceirosApp => Set<ParceiroApp>();

        protected override void OnModelCreating(ModelBuilder b) { 
            base.OnModelCreating(b);

            b.Entity<ItemCardapio>()
                .HasMany(i => i.Ingredientes)
                .WithMany(i => i.Itens);

            b.Entity<Reserva>()
                .HasIndex(r => new { r.MesaId, r.DataHora })
                .IsUnique();

            b.Entity<PerfilCliente>()
                .HasIndex(p => p.UsuarioId)
                .IsUnique(); // 1–1 com usuário

            b.Entity<Mesa>()
                .HasIndex(m => m.Numero)
                .IsUnique();

            b.Entity<SugestaoDoChefe>()
                .HasIndex(s => new { s.Data, s.Periodo })
                .IsUnique(); // 1 sugestão por dia e por período
                             // opcoes para Atendimento
            b.Entity<Atendimento>()
             .HasDiscriminator<string>("Discriminator")
             .HasValue<AtendimentoPresencial>("Presencial")
             .HasValue<AtendimentoDeliveryProprio>("DeliveryProprio")
             .HasValue<AtendimentoDeliveryAplicativo>("DeliveryAplicativo");

            // 1–1 pedido <-> atendimento
            b.Entity<Pedido>()
             .HasOne(p => p.Atendimento)
             .WithOne(a => a.Pedido)
             .HasForeignKey<Pedido>(p => p.AtendimentoId)
             .OnDelete(DeleteBehavior.Restrict);

            b.Entity<ItemCardapio>().Property(i => i.PrecoBase).HasPrecision(18, 2);
            b.Entity<PedidoItem>().Property(i => i.PrecoUnitarioBase).HasPrecision(18, 2);
            b.Entity<PedidoItem>().Property(i => i.DescontoUnitarioAplicado).HasPrecision(18, 2);
            b.Entity<Pedido>().Property(p => p.SubtotalItens).HasPrecision(18, 2);
            b.Entity<Pedido>().Property(p => p.TotalDescontos).HasPrecision(18, 2);
            b.Entity<Pedido>().Property(p => p.TotalTaxas).HasPrecision(18, 2);
            b.Entity<Pedido>().Property(p => p.TotalFinal).HasPrecision(18, 2);

            b.Entity<EnderecoEntrega>()
                .HasOne(e => e.Perfil)
                .WithMany(p => p.Enderecos)
                .HasForeignKey(e => e.PerfilClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
}
}
