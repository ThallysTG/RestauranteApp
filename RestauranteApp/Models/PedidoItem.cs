using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class PedidoItem
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = default!;

        public int ItemCardapioId { get; set; }
        public ItemCardapio Item { get; set; } = default!;

        [Range(1, 999)]
        public int Quantidade { get; set; }

        // guardamos o preço e desconto aplicados no momento
        public decimal PrecoUnitarioBase { get; set; }
        public decimal DescontoUnitarioAplicado { get; set; } // 0 ou 20% do unitário seguindo a logica
    }
}
