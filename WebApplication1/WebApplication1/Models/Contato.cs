// Em Models/Contato.cs
namespace WebApplication1.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Contato
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }
        public string? Empresa { get; set; }
        public List<string> Emails { get; set; } = new List<string>();
        public string? TelefonePessoal { get; set; }
        public string? TelefoneComercial { get; set; }
    }
}