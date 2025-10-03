using Microsoft.EntityFrameworkCore;
using RestauranteApp.Models;

namespace RestauranteApp.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // garante que o banco existe / aplica migrations em dev
        await ctx.Database.MigrateAsync();

        // INGREDIENTES
        if (!ctx.Ingredientes.Any())
        {
            var ingredientes = new[]
            {
                new Ingrediente { Nome = "Arroz" },
                new Ingrediente { Nome = "Feijão" },
                new Ingrediente { Nome = "Batata" },
                new Ingrediente { Nome = "Carne" },
                new Ingrediente { Nome = "Frango" },
                new Ingrediente { Nome = "Peixe" },
                new Ingrediente { Nome = "Ovo" },
                new Ingrediente { Nome = "Queijo" },
                new Ingrediente { Nome = "Tomate" },
                new Ingrediente { Nome = "Alface" },
                new Ingrediente { Nome = "Cebola" },
                new Ingrediente { Nome = "Molho" }
            };
            ctx.Ingredientes.AddRange(ingredientes);
            await ctx.SaveChangesAsync();
        }

        // ITENS DE CARDÁPIO
        if (!ctx.ItensCardapio.Any())
        {
            decimal Preco(decimal basePrice, int i) => basePrice + (i % 5) * 2; // só pra variar
            var almoco = Enumerable.Range(1, 20).Select(i => new ItemCardapio
            {
                Nome = $"Prato Almoço {i}",
                Descricao = $"Prato de almoço #{i}",
                PrecoBase = Preco(18, i),
                Periodo = Periodo.Almoco
            });
            var jantar = Enumerable.Range(1, 20).Select(i => new ItemCardapio
            {
                Nome = $"Prato Jantar {i}",
                Descricao = $"Prato de jantar #{i}",
                PrecoBase = Preco(22, i),
                Periodo = Periodo.Jantar
            });

            ctx.ItensCardapio.AddRange(almoco);
            ctx.ItensCardapio.AddRange(jantar);
            await ctx.SaveChangesAsync();

            // vincula 2–4 ingredientes aleatórios por item
            var rnd = new Random();
            var allIngIds = ctx.Ingredientes.Select(x => x.Id).ToList();
            var allItens = ctx.ItensCardapio.AsNoTracking().ToList();

            
        }

        // Sugestão do Chefe (exemplo para HOJE) 
        var hoje = DateOnly.FromDateTime(DateTime.Now.Date);

        // se não existir sugestão para hoje, cria uma de cada período só para demonstrar
        if (!ctx.SugestoesDoChefe.Any(s => s.Data == hoje && s.Periodo == Periodo.Almoco))
        {
            var itemAlmoco = ctx.ItensCardapio.First(i => i.Periodo == Periodo.Almoco);
            ctx.SugestoesDoChefe.Add(new SugestaoDoChefe
            {
                Data = hoje,
                Periodo = Periodo.Almoco,
                ItemCardapioId = itemAlmoco.Id
            });
        }

        if (!ctx.SugestoesDoChefe.Any(s => s.Data == hoje && s.Periodo == Periodo.Jantar))
        {
            var itemJantar = ctx.ItensCardapio.First(i => i.Periodo == Periodo.Jantar);
            ctx.SugestoesDoChefe.Add(new SugestaoDoChefe
            {
                Data = hoje,
                Periodo = Periodo.Jantar,
                ItemCardapioId = itemJantar.Id
            });
        }

        await ctx.SaveChangesAsync();
    }
}
