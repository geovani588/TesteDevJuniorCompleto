namespace MinhaApiComSQLite.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        
        // Relacionamento: O produto pertence a uma categoria
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}