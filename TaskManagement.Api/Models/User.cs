namespace TaskManagement.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Outras propriedades que podem ser relevantes para um usuário
        public string Role { get; set; }  // Exemplo de uma propriedade para definir a função do usuário (e.g., admin, gerente, etc.)
    }
}
