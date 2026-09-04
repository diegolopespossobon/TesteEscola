using System.Net;
using System.Web.Http;
using TesteEscola.Api.Dtos;
using TesteEscola.Api.Infrastructure;
using TesteEscola.Api.Services;

namespace TesteEscola.Api.Controllers
{
    [RoutePrefix("api/alunos")]
    public class AlunosController : ApiController
    {
        private readonly AlunoService _alunoService;

        public AlunosController()
            : this(ServiceResolver.AlunoService)
        {
        }

        public AlunosController(AlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(string nome = null, int page = 1, int pageSize = 10)
        {
            return Ok(_alunoService.Listar(nome, page, pageSize));
        }

        [HttpGet]
        [Route("{id:int}", Name = "ObterAlunoPorId")]
        public IHttpActionResult Get(int id)
        {
            var aluno = _alunoService.ObterPorId(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return Ok(aluno);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(AlunoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _alunoService.Criar(request);
            var aluno = _alunoService.ObterPorId(id);
            return CreatedAtRoute("ObterAlunoPorId", new { id = id }, aluno);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, AlunoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_alunoService.Atualizar(id, request))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_alunoService.Inativar(id))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
