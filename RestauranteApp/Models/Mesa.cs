namespace RestauranteApp.Models
{
    public class Mesa
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
