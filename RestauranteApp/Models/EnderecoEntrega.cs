using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class EnderecoEntrega
    {
        public int Id { get; set; }

        public int PerfilClienteId { get; set; }
        public PerfilCliente? Perfil { get; set; }

        [Required, StringLength(40)]
        public string Titulo { get; set; } = "Casa"; // apelido

        [Required, StringLength(120)]
        public string Logradouro { get; set; } = "";

        [Required, StringLength(20)]
        public string Numero { get; set; } = "";

        [StringLength(60)]
        public string? Complemento { get; set; }

        [Required, StringLength(60)]
        public string Bairro { get; set; } = "";

        [Required, StringLength(60)]
        public string Cidade { get; set; } = "";

        [Required, StringLength(2)]
        public string UF { get; set; } = "SP";

        [Required, StringLength(9)]
        public string CEP { get; set; } = ""; 
    }
}
