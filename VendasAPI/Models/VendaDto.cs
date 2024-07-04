namespace VendasAPI.Models
{
    public class VendaDto
    {
        public int IdVenda { get; set; }
        public int IdCliente { get; set; }
        public string ClienteNome { get; set; }
        public int IdProduto { get; set; }
        public string ProdutoNome { get; set; }
        public int QtdVenda { get; set; }
        public decimal VlrUnitarioVenda { get; set; }
        public DateTime DthVenda { get; set; }
        public decimal VlrTotalVenda { get; set; }
    }
}
