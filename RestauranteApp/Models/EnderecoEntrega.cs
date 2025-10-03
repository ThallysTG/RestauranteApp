namespace RestauranteApp.Models
{
    public class EnderecoEntrega
    {
        public int Id { get; set; }
        public int PerfilClienteId { get; set; }
        public PerfilCliente Perfil { get; set; } = default!;
        public string Titulo { get; set; } = "Casa"; // apelido
        public string Logradouro { get; set; } = "";
        public string Numero { get; set; } = "";
        public string? Complemento { get; set; }
        public string Bairro { get; set; } = "";
        public string Cidade { get; set; } = "";
        public string UF { get; set; } = "SP";
        public string CEP { get; set; } = "";
    }
}
