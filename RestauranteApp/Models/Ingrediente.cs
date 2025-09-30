namespace RestauranteApp.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<ItemCardapio> Itens { get; set; } = new List<ItemCardapio>();
    }
}
