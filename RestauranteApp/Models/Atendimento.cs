using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteApp.Models
{
    public abstract class Atendimento
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Apenas para exibição; não persiste no banco
        [NotMapped]
        public abstract string Tipo { get; }

        // 1–1 com Pedido
        public Pedido? Pedido { get; set; }
    }

    public class AtendimentoPresencial : Atendimento
    {
        public override string Tipo => "Presencial";
    }

    public class AtendimentoDeliveryProprio : Atendimento
    {
        public decimal TaxaFixa { get; set; }
        public override string Tipo => "Delivery Próprio";
    }

    public class AtendimentoDeliveryAplicativo : Atendimento
    {
        public string Parceiro { get; set; } = "";
        public decimal? TaxaFixa { get; set; }
        public decimal ComissaoPercentual { get; set; }
        public override string Tipo => "Delivery App";
    }
}
