using System.Collections.Generic;
using System.Linq;
using VendasAPI.Data;
using VendasAPI.Models;

namespace VendasAPI.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cliente> GetAllClientes()
        {
            return _context.Clientes.ToList();
        }

        public Cliente GetClienteById(int id)
        {
            return _context.Clientes.Find(id);
        }

        public IEnumerable<Cliente> GetClientesByName(string nome)
        {
            return _context.Clientes.Where(c => c.Nome.Contains(nome)).ToList();
        }

        public Cliente AddCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return cliente;
        }

        public Cliente UpdateCliente(Cliente cliente)
        {
            var clienteExistente = _context.Clientes.Find(cliente.Id);
            if (clienteExistente == null)
            {
                throw new ArgumentException("Cliente não encontrado");
            }

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Cidade = cliente.Cidade;

            _context.Clientes.Update(clienteExistente);
            _context.SaveChanges();
            return clienteExistente;
        }

        public bool DeleteCliente(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return false;
            }

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            return true;
        }
    }
}
