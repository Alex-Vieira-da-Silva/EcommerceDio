using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecommerce_microservico.microservico_estoque.Entities;
using ecommerce_microservico.microservico_estoque.Services;

namespace ecommerce_microservico.Controllers
{
    [ApiController]
    [Route("api/v1/estoque/produtos")]
    public class ProdutosController : ControllerBase
    {
        private readonly IEstoqueService _estoqueService;

        public ProdutosController(IEstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> ObterTodos()
        {
            var produtos = await _estoqueService.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Produto>> GetById(Guid id)
        {
            var produto = await _estoqueService.GetByIdAsync(id);
            if (produto is null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Criar([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var criado = await _estoqueService.CreateAsync(produto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = criado.Id },
                criado
            );
        }

        [HttpPut("{id:guid}/reserva/{quantidade:int}")]
        public async Task<IActionResult> Reserva(Guid id, int quantidade)
        {
            var sucesso = await _estoqueService.ReservaAsync(id, quantidade);
            if (!sucesso)
                return BadRequest("Estoque insuficiente.");

            return NoContent();
        }
    }
}