using System.ComponentModel.DataAnnotations;

namespace MinhaApiComSQLite.DTOs
{
    // DTO para a Criação e Atualização de Produtos (Entrada de dados)
    public class ProdutoCreateDto
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [MinLength(2, ErrorMessage = "O nome do produto deve ser mais descritivo.")]
        public string Nome { get; set; } = string.Empty;

        // Note que aceitamos double/decimal aqui, mas validaremos de forma rigorosa no Service
        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
        public int CategoriaId { get; set; }
    }

    // DTO para Retorno de Produtos na API (Saída de dados limpa)
    public class ProdutoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNome { get; set; } // Diferencial: exibe o nome da categoria, não só o ID
    }

    // DTO para o relatório de estatísticas exigido no teste
    public class RelatorioEstatisticasDto
    {
        public int TotalProdutos { get; set; }
        public decimal PrecoMedio { get; set; }
        public decimal ValorTotalEstoque { get; set; }
    }
}