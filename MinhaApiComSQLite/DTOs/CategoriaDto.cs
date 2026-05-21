using System.ComponentModel.DataAnnotations;

namespace MinhaApiComSQLite.DTOs
{
    // DTO utilizado para receber os dados na criação/atualização de uma categoria
    public class CategoriaCreateDto
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        public string Nome { get; set; } = string.Empty;
    }

    // DTO utilizado para retornar os dados de uma categoria pela API
    public class CategoriaResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}