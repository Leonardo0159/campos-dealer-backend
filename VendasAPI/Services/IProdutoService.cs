using System.Collections.Generic;
using VendasAPI.Models;

namespace VendasAPI.Services
{
    public interface IProdutoService
    {
        IEnumerable<Produto> GetAllProdutos();
        Produto GetProdutoById(int id);
        IEnumerable<Produto> GetProdutosByName(string nome);
        Produto AddProduto(Produto produto);
        Produto UpdateProduto(Produto produto);
        bool DeleteProduto(int id);
    }
}
