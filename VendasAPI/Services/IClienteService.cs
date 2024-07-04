using System.Collections.Generic;
using VendasAPI.Models;

namespace VendasAPI.Services
{
    public interface IClienteService
    {
        IEnumerable<Cliente> GetAllClientes();
        Cliente GetClienteById(int id);
        IEnumerable<Cliente> GetClientesByName(string nome);
        Cliente AddCliente(Cliente cliente);
        Cliente UpdateCliente(Cliente cliente);
        bool DeleteCliente(int id);
    }
}
