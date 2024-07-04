using System.Collections.Generic;
using System.Linq;
using VendasAPI.Data;
using VendasAPI.Models;
using System;

namespace VendasAPI.Services
{
    public class VendaService : IVendaService
    {
        private readonly ApplicationDbContext _context;

        public VendaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<VendaDto> GetAllVendas()
        {
            var vendas = from venda in _context.Vendas
                         join cliente in _context.Clientes on venda.IdCliente equals cliente.Id
                         join produto in _context.Produtos on venda.IdProduto equals produto.Id
                         select new VendaDto
                         {
                             IdVenda = venda.IdVenda,
                             IdCliente = cliente.Id,
                             ClienteNome = cliente.Nome,
                             IdProduto = produto.Id,
                             ProdutoNome = produto.Nome,
                             QtdVenda = venda.QtdVenda,
                             VlrUnitarioVenda = venda.VlrUnitarioVenda,
                             DthVenda = venda.DthVenda,
                             VlrTotalVenda = venda.VlrTotalVenda
                         };

            return vendas.ToList();
        }

        public Venda GetVendaById(int id)
        {
            return _context.Vendas.Find(id);
        }

        public IEnumerable<VendaDto> GetVendasByClienteOrProduto(string nome)
        {
            var vendas = from venda in _context.Vendas
                         join cliente in _context.Clientes on venda.IdCliente equals cliente.Id
                         join produto in _context.Produtos on venda.IdProduto equals produto.Id
                         where cliente.Nome.Contains(nome) || produto.Nome.Contains(nome)
                         select new VendaDto
                         {
                             IdVenda = venda.IdVenda,
                             IdCliente = cliente.Id,
                             ClienteNome = cliente.Nome,
                             IdProduto = produto.Id,
                             ProdutoNome = produto.Nome,
                             QtdVenda = venda.QtdVenda,
                             VlrUnitarioVenda = venda.VlrUnitarioVenda,
                             DthVenda = venda.DthVenda,
                             VlrTotalVenda = venda.VlrTotalVenda
                         };

            return vendas.ToList();
        }

        public Venda AddVenda(Venda venda)
        {
            var cliente = _context.Clientes.Find(venda.IdCliente);
            if (cliente == null)
            {
                throw new ArgumentException("Cliente não encontrado");
            }

            var produto = _context.Produtos.Find(venda.IdProduto);
            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado");
            }

            if (venda.QtdVenda <= 0)
            {
                throw new ArgumentException("Quantidade de venda deve ser maior que 0");
            }

            venda.VlrUnitarioVenda = produto.Preco;
            venda.DthVenda = DateTime.UtcNow;
            venda.VlrTotalVenda = venda.QtdVenda * venda.VlrUnitarioVenda;

            _context.Vendas.Add(venda);
            _context.SaveChanges();
            return venda;
        }

        public Venda UpdateVenda(Venda venda)
        {
            var vendaExistente = _context.Vendas.Find(venda.IdVenda);
            if (vendaExistente == null)
            {
                throw new ArgumentException("Venda não encontrada");
            }

            var cliente = _context.Clientes.Find(venda.IdCliente);
            if (cliente == null)
            {
                throw new ArgumentException("Cliente não encontrado");
            }

            var produto = _context.Produtos.Find(venda.IdProduto);
            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado");
            }

            if (venda.QtdVenda <= 0)
            {
                throw new ArgumentException("Quantidade de venda deve ser maior que 0");
            }

            vendaExistente.IdCliente = venda.IdCliente;
            vendaExistente.IdProduto = venda.IdProduto;
            vendaExistente.QtdVenda = venda.QtdVenda;
            vendaExistente.VlrUnitarioVenda = produto.Preco;
            vendaExistente.VlrTotalVenda = venda.QtdVenda * produto.Preco;
            vendaExistente.DthVenda = vendaExistente.DthVenda;

            _context.Vendas.Update(vendaExistente);
            _context.SaveChanges();
            return vendaExistente;
        }

        public bool DeleteVenda(int id)
        {
            var venda = _context.Vendas.Find(id);
            if (venda == null)
            {
                return false;
            }

            _context.Vendas.Remove(venda);
            _context.SaveChanges();
            return true;
        }
    }
}
