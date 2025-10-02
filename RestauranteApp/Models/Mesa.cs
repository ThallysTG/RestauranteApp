using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Mesa
    {
        public int Id { get; set; }
        [Range(1, 999, ErrorMessage = "Informe um número de mesa válido")]
        public int Numero { get; set; }
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
