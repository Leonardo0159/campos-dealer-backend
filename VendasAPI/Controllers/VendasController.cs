using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using VendasAPI.Models;
using VendasAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace VendasAPI.Controllers
{
    [Route("api/vendas")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VendaDto>> GetAll()
        {
            var vendas = _vendaService.GetAllVendas();
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public ActionResult<Venda> GetById(int id)
        {
            var venda = _vendaService.GetVendaById(id);
            if (venda == null)
            {
                return NotFound();
            }

            return Ok(venda);
        }

        [HttpGet("buscar/{nome}")]
        public ActionResult<IEnumerable<VendaDto>> GetVendasByClienteOrProduto(string nome)
        {
            var vendas = _vendaService.GetVendasByClienteOrProduto(nome);
            if (!vendas.Any())
            {
                return NotFound();
            }
            return Ok(vendas);
        }

        [HttpPost]
        public ActionResult<Venda> Add([FromBody] Venda venda)
        {
            try
            {
                var novaVenda = _vendaService.AddVenda(venda);
                return CreatedAtAction(nameof(GetById), new { id = novaVenda.IdVenda }, novaVenda);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Venda> Update(int id, [FromBody] Venda venda)
        {
            try
            {
                var vendaExistente = _vendaService.GetVendaById(id);
                if (vendaExistente == null)
                {
                    return NotFound();
                }

                vendaExistente.IdCliente = venda.IdCliente;
                vendaExistente.IdProduto = venda.IdProduto;
                vendaExistente.QtdVenda = venda.QtdVenda;
                vendaExistente.VlrUnitarioVenda = venda.VlrUnitarioVenda;
                vendaExistente.VlrTotalVenda = venda.QtdVenda * venda.VlrUnitarioVenda;

                var vendaAtualizada = _vendaService.UpdateVenda(vendaExistente);
                return Ok(vendaAtualizada);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var sucesso = _vendaService.DeleteVenda(id);
            if (!sucesso)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
