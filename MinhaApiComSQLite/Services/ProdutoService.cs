using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Data;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly AppDbContext _db;

        public ProdutoService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProdutoResponseDto>> GetProdutosAsync(int pageNumber, int pageSize)
        {
            var produtos = await _db.Produtos
                .Include(p => p.Categoria)
                .OrderBy(p => p.Preco)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return produtos.Select(p => new ProdutoResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Preco = p.Preco,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria?.Nome
            });
        }

        public async Task<ProdutoResponseDto?> GetByIdAsync(int id)
        {
            var p = await _db.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null)
                return null;

            return new ProdutoResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Preco = p.Preco,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria?.Nome
            };
        }

        public async Task<(bool Success, string ErrorMessage, ProdutoResponseDto? Data)> CreateAsync(ProdutoCreateDto dto)
        {
            if (dto.Preco <= 0)
                return (false, "O preço deve ser maior que zero.", null);

            var produto = new Produto
            {
                Nome = dto.Nome.Trim().ToUpper(),
                Preco = dto.Preco,
                CategoriaId = dto.CategoriaId
            };

            _db.Produtos.Add(produto);

            await _db.SaveChangesAsync();

            return (true, "", new ProdutoResponseDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                CategoriaId = produto.CategoriaId
            });
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateAsync(int id, ProdutoCreateDto dto)
        {
            var produto = await _db.Produtos.FindAsync(id);

            if (produto == null)
                return (false, "Produto não encontrado");

            produto.Nome = dto.Nome.Trim().ToUpper();
            produto.Preco = dto.Preco;
            produto.CategoriaId = dto.CategoriaId;

            await _db.SaveChangesAsync();

            return (true, "");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var produto = await _db.Produtos.FindAsync(id);

            if (produto == null)
                return false;

            _db.Produtos.Remove(produto);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<RelatorioEstatisticasDto> GetEstatisticasAsync()
        {
            var total = await _db.Produtos.CountAsync();

            var media = total > 0
                ? await _db.Produtos.AverageAsync(p => p.Preco)
                : 0;

            var estoque = total > 0
                ? await _db.Produtos.SumAsync(p => p.Preco)
                : 0;

            return new RelatorioEstatisticasDto
            {
                TotalProdutos = total,
                PrecoMedio = Math.Round(media, 2),
                ValorTotalEstoque = estoque
            };
        }

        public decimal CalcularDescontoProgressivo(decimal preco, int quantidade)
        {
            var desconto = quantidade switch
            {
                > 20 => 0.15m,
                > 10 => 0.10m,
                > 5 => 0.05m,
                _ => 0m
            };

            return preco * (1 - desconto) * quantidade;
        }
    }
}