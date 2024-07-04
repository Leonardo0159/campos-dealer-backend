using System.Collections.Generic;
using System.Linq;
using VendasAPI.Data;
using VendasAPI.Models;

namespace VendasAPI.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ApplicationDbContext _context;

        public ProdutoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> GetAllProdutos()
        {
            return _context.Produtos.ToList();
        }

        public Produto GetProdutoById(int id)
        {
            return _context.Produtos.Find(id);
        }

        public IEnumerable<Produto> GetProdutosByName(string nome)
        {
            return _context.Produtos.Where(p => p.Nome.Contains(nome)).ToList();
        }

        public Produto AddProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto UpdateProduto(Produto produto)
        {
            var produtoExistente = _context.Produtos.Find(produto.Id);
            if (produtoExistente == null)
            {
                throw new ArgumentException("Produto não encontrado");
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Preco = produto.Preco;

            _context.Produtos.Update(produtoExistente);
            _context.SaveChanges();
            return produtoExistente;
        }

        public bool DeleteProduto(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null)
            {
                return false;
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return true;
        }
    }
}
