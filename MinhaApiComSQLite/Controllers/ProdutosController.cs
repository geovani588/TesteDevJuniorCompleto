using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Services;

namespace MinhaApiComSQLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutosController(ProdutoService service) => _service = service;

        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (page <= 0 || size <= 0) return BadRequest("Página e tamanho devem ser maiores que zero.");
            
            var produtos = await _service.GetProdutosAsync(page, size);
            return Ok(produtos);
        }

        // BUSCA POR ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _service.GetByIdAsync(id);
            return produto == null ? NotFound(new { mensagem = "Produto não encontrado." }) : Ok(produto);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProdutoCreateDto dto)
        {
            var (success, error, data) = await _service.CreateAsync(dto);
            
            if (!success) 
                return BadRequest(new { mensagem = error });

            return CreatedAtAction(nameof(GetById), new { id = data!.Id }, data);
        }

        // ATUALIZAÇÃO
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProdutoCreateDto dto)
        {
            var (success, error) = await _service.UpdateAsync(id, dto);

            if (!success)
            {
                if (error == "Not Found") return NotFound(new { mensagem = "Produto não encontrado." });
                return BadRequest(new { mensagem = error });
            }

            return NoContent();
        }

        // EXCLUSÃO
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletado = await _service.DeleteAsync(id);
            return !deletado ? NotFound(new { mensagem = "Produto não encontrado." }) : NoContent();
        }

        // ENDPOINT EXTRA: Relatório de Estatísticas
        [HttpGet("estatisticas")]
        public async Task<IActionResult> GetEstatisticas()
        {
            var dados = await _service.GetEstatisticasAsync();
            return Ok(dados);
        }

        [HttpGet("simular-desconto")]
        public IActionResult SimularDesconto([FromQuery] decimal preco, [FromQuery] int quantidade)
        {
            if (preco <= 0 || quantidade <= 0) return BadRequest("Preço e quantidade devem ser positivos.");
            
            // Aqui passamos os parâmetros direto, sem o 'quantity:', evitando o erro!
            var total = _service.CalcularDescontoProgressivo(preco, quantidade);
            return Ok(new { precoOriginal = preco, qtd = quantidade, valorTotalComDesconto = total });
        }
    }
}