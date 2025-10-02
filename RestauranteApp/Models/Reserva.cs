using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public int NumeroPessoas { get; set; }
        [Display(Name = "Mesa")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma mesa.")]
        public int MesaId { get; set; }
        public Mesa? Mesa { get; set; }
        public string UsuarioId { get; set; }
    }
}
