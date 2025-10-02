namespace RestauranteApp.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public int NumeroPessoas { get; set; }
        public int MesaId { get; set; }
        public Mesa? Mesa { get; set; }
        public string UsuarioId { get; set; }
    }
}
