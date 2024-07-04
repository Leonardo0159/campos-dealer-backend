using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VendasAPI.Models;
using VendasAPI.Services;

namespace VendasAPI.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetAll()
        {
            var produtos = _produtoService.GetAllProdutos();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _produtoService.GetProdutoById(id);
            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpGet("nome/{nome}")]
        public ActionResult<IEnumerable<Produto>> GetByName(string nome)
        {
            var produtos = _produtoService.GetProdutosByName(nome);
            if (!produtos.Any())
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        [HttpPost]
        public ActionResult<Produto> Add([FromBody] Produto produto)
        {
            var novoProduto = _produtoService.AddProduto(produto);
            return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
        }

        [HttpPut("{id}")]
        public ActionResult<Produto> Update(int id, [FromBody] Produto produto)
        {
            var produtoExistente = _produtoService.GetProdutoById(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Preco = produto.Preco;

            var produtoAtualizado = _produtoService.UpdateProduto(produtoExistente);
            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var sucesso = _produtoService.DeleteProduto(id);
            if (!sucesso)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
