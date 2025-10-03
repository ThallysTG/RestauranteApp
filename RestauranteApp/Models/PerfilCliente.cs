using Microsoft.AspNetCore.Identity;

namespace RestauranteApp.Models
{
    public class PerfilCliente
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = default!;
        public IdentityUser Usuario { get; set; } = default!;
        public string NomeCompleto { get; set; } = "";
        public string? Telefone { get; set; }
        public ICollection<EnderecoEntrega> Enderecos { get; set; } = new List<EnderecoEntrega>();
    }
}
