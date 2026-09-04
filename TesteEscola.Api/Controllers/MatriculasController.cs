using System.Net;
using System.Web.Http;
using TesteEscola.Api.Dtos;
using TesteEscola.Api.Infrastructure;
using TesteEscola.Api.Repositories;
using TesteEscola.Api.Services;

namespace TesteEscola.Api.Controllers
{
    [RoutePrefix("api/matriculas")]
    public class MatriculasController : ApiController
    {
        private readonly MatriculaService _matriculaService;

        public MatriculasController()
            : this(ServiceResolver.MatriculaService)
        {
        }

        public MatriculasController(MatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(MatriculaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _matriculaService.Matricular(request);

            switch (result.Status)
            {
                case MatriculaStatus.Sucesso:
                    return Content(HttpStatusCode.Created, new { id = result.Id });
                case MatriculaStatus.AlunoNaoEncontradoOuInativo:
                    return Content(HttpStatusCode.Conflict, new { mensagem = "Aluno inexistente ou inativo." });
                case MatriculaStatus.TurmaNaoEncontrada:
                    return NotFound();
                case MatriculaStatus.TurmaSemVaga:
                    return Content(HttpStatusCode.Conflict, new { mensagem = "Turma sem vaga disponivel." });
                case MatriculaStatus.MatriculaDuplicada:
                    return Content(HttpStatusCode.Conflict, new { mensagem = "Aluno ja matriculado nesta turma." });
                default:
                    return InternalServerError();
            }
        }
    }
}
