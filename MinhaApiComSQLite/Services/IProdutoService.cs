using MinhaApiComSQLite.DTOs;

namespace MinhaApiComSQLite.Services
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoResponseDto>> GetProdutosAsync(int pageNumber, int pageSize);

        Task<ProdutoResponseDto?> GetByIdAsync(int id);

        Task<(bool Success, string ErrorMessage, ProdutoResponseDto? Data)> CreateAsync(ProdutoCreateDto dto);

        Task<(bool Success, string ErrorMessage)> UpdateAsync(int id, ProdutoCreateDto dto);

        Task<bool> DeleteAsync(int id);

        Task<RelatorioEstatisticasDto> GetEstatisticasAsync();

        decimal CalcularDescontoProgressivo(decimal preco, int quantidade);
    }
}