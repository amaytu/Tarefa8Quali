// Em Controllers/ContatosController.cs
namespace WebApplication1.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Models;
    using WebApplication1.Services;

    [ApiController] // Indica que este é um controller de API
    [Route("api/contatos")] // Define a rota base: https://localhost:porta/api/contatos
    public class ContatosController : ControllerBase
    {
        private readonly ContatoService _contatoService;

        public ContatosController(ContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        // GET: api/contatos?filtro=texto
        [HttpGet]
        public ActionResult<List<Contato>> GetContatos([FromQuery] string? filtro)
        {
            return Ok(_contatoService.ObterTodos(filtro));
        }

        // GET: api/contatos/5
        [HttpGet("{id}")]
        public ActionResult<Contato> GetContato(int id)
        {
            var contato = _contatoService.ObterPorId(id);
            if (contato == null)
            {
                return NotFound(); // Retorna código 404
            }
            return Ok(contato); // Retorna o contato e código 200
        }

        // POST: api/contatos
        [HttpPost]
        public ActionResult<Contato> PostContato([FromBody] Contato contato)
        {
            contato.Emails.RemoveAll(string.IsNullOrWhiteSpace);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var novoContato = _contatoService.Adicionar(contato);
            return CreatedAtAction(nameof(GetContato), new { id = novoContato.Id }, novoContato); // Retorna 201 Created
        }

        // PUT: api/contatos/5
        [HttpPut("{id}")]
        public IActionResult PutContato(int id, [FromBody] Contato contato)
        {
            if (id != contato.Id)
            {
                return BadRequest();
            }
            contato.Emails.RemoveAll(string.IsNullOrWhiteSpace);
            _contatoService.Atualizar(contato);
            return NoContent(); // Retorna 204 No Content
        }

        // DELETE: api/contatos/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContato(int id)
        {
            var contato = _contatoService.ObterPorId(id);
            if (contato == null)
            {
                return NotFound();
            }
            _contatoService.Excluir(id);
            return NoContent(); // Retorna 204 No Content
        }
    }
}