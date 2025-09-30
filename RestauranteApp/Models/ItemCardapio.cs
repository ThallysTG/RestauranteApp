using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteApp.Models
{
    public enum Periodo
    {
        Almoco = 1,
        Jantar = 2
    }
    public class ItemCardapio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoBase { get; set; }
        public Periodo Periodo { get; set; }

        public ICollection<Ingrediente> Ingredientes { get; set; } = new List<Ingrediente>();

    }
}
