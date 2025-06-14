// Em Services/ContatoService.cs
namespace WebApplication1.Services
{
    using WebApplication1.Models;

    // A lógica desta classe continua exatamente a mesma da resposta anterior.
    // Cole aqui o código do ContatoService, apenas garantindo que o namespace e o 'using' estejam corretos.
    public class ContatoService
    {
        private static List<Contato> _contatos = new List<Contato>();
        private static int _proximoId = 1;

        public List<Contato> ObterTodos(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                return _contatos;
            }
            return _contatos.Where(c =>
                c.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                (c.Empresa != null && c.Empresa.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                (c.TelefonePessoal != null && c.TelefonePessoal.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                (c.TelefoneComercial != null && c.TelefoneComercial.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                c.Emails.Any(email => email.Contains(filtro, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public Contato? ObterPorId(int id) => _contatos.FirstOrDefault(c => c.Id == id);

        public Contato Adicionar(Contato contato)
        {
            contato.Id = _proximoId++;
            _contatos.Add(contato);
            return contato;
        }

        public void Atualizar(Contato contatoAtualizado)
        {
            var contatoExistente = _contatos.FirstOrDefault(c => c.Id == contatoAtualizado.Id);
            if (contatoExistente != null)
            {
                contatoExistente.Nome = contatoAtualizado.Nome;
                contatoExistente.Empresa = contatoAtualizado.Empresa;
                contatoExistente.Emails = contatoAtualizado.Emails;
                contatoExistente.TelefonePessoal = contatoAtualizado.TelefonePessoal;
                contatoExistente.TelefoneComercial = contatoAtualizado.TelefoneComercial;
            }
        }

        public void Excluir(int id)
        {
            var contato = _contatos.FirstOrDefault(c => c.Id == id);
            if (contato != null)
            {
                _contatos.Remove(contato);
            }
        }
    }
}