namespace RestauranteApp.Models
{
    public class ItemCardapioIngrediente
    {
        public int ItemCardapioId { get; set; }
        public ItemCardapio? ItemCardapio { get; set; }
        public int IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; }
    }
}
