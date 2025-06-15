namespace WebApplication1.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Models;
    using WebApplication1.Services;

    [ApiController] 
    [Route("api/contatos")]     public class ContatosController : ControllerBase
    {
        private readonly ContatoService _contatoService;

        public ContatosController(ContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [HttpGet]
        public ActionResult<List<Contato>> GetContatos([FromQuery] string? filtro)
        {
            return Ok(_contatoService.ObterTodos(filtro));
        }

        [HttpGet("{id}")]
        public ActionResult<Contato> GetContato(int id)
        {
            var contato = _contatoService.ObterPorId(id);
            if (contato == null)
            {
                return NotFound(); 
            }
            return Ok(contato); 
        }

        [HttpPost]
        public ActionResult<Contato> PostContato([FromBody] Contato contato)
        {
            contato.Emails.RemoveAll(string.IsNullOrWhiteSpace);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var novoContato = _contatoService.Adicionar(contato);
            return CreatedAtAction(nameof(GetContato), new { id = novoContato.Id }, novoContato); 
        }

        [HttpPut("{id}")]
        public IActionResult PutContato(int id, [FromBody] Contato contato)
        {
            if (id != contato.Id)
            {
                return BadRequest();
            }
            contato.Emails.RemoveAll(string.IsNullOrWhiteSpace);
            _contatoService.Atualizar(contato);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContato(int id)
        {
            var contato = _contatoService.ObterPorId(id);
            if (contato == null)
            {
                return NotFound();
            }
            _contatoService.Excluir(id);
            return NoContent(); 
        }
    }
}