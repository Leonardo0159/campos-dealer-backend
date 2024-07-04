using System;
using System.ComponentModel.DataAnnotations;

namespace VendasAPI.Models
{
    public class Venda
    {
        [Key]
        public int IdVenda { get; set; }
        public int IdCliente { get; set; }
        public int IdProduto { get; set; }
        public int QtdVenda { get; set; }
        public decimal VlrUnitarioVenda { get; set; }
        public DateTime DthVenda { get; set; }
        public decimal VlrTotalVenda { get; set; }
    }
}
