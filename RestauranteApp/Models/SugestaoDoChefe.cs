using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class SugestaoDoChefe
    {
        public int Id { get; set; }
        public DateOnly Data { get; set; }         // dia da sugestão
        public Periodo Periodo { get; set; }       // Almoco ou Jantar
        [Display(Name = "Item do Cardápio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um item do cardápio.")]
        public int ItemCardapioId { get; set; }    // item escolhido
        [ValidateNever]
        public ItemCardapio? Item { get; set; } = default!;
    }
}
