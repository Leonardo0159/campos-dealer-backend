using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VendasAPI.Models;
using VendasAPI.Services;

namespace VendasAPI.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetAll()
        {
            var clientes = _clienteService.GetAllClientes();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> GetById(int id)
        {
            var cliente = _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpGet("nome/{nome}")]
        public ActionResult<IEnumerable<Cliente>> GetByName(string nome)
        {
            var clientes = _clienteService.GetClientesByName(nome);
            if (!clientes.Any())
            {
                return NotFound();
            }
            return Ok(clientes);
        }

        [HttpPost]
        public ActionResult<Cliente> Add([FromBody] Cliente cliente)
        {
            var novoCliente = _clienteService.AddCliente(cliente);
            return CreatedAtAction(nameof(GetById), new { id = novoCliente.Id }, novoCliente);
        }

        [HttpPut("{id}")]
        public ActionResult<Cliente> Update(int id, [FromBody] Cliente cliente)
        {
            var clienteExistente = _clienteService.GetClienteById(id);
            if (clienteExistente == null)
            {
                return NotFound();
            }

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Cidade = cliente.Cidade;

            var clienteAtualizado = _clienteService.UpdateCliente(clienteExistente);
            return Ok(clienteAtualizado);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var sucesso = _clienteService.DeleteCliente(id);
            if (!sucesso)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
