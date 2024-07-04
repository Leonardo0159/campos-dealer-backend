using System.Collections.Generic;
using VendasAPI.Models;

namespace VendasAPI.Services
{
    public interface IVendaService
    {
        IEnumerable<VendaDto> GetAllVendas();
        Venda GetVendaById(int id);
        IEnumerable<VendaDto> GetVendasByClienteOrProduto(string nome);
        Venda AddVenda(Venda venda);
        Venda UpdateVenda(Venda venda);
        bool DeleteVenda(int id);
    }
}
