using System.Web.Http;
using TesteEscola.Api.Infrastructure;
using TesteEscola.Api.Repositories;

namespace TesteEscola.Api.Controllers
{
    [RoutePrefix("api/relatorios")]
    public class RelatoriosController : ApiController
    {
        private readonly IRelatorioRepository _relatorioRepository;

        public RelatoriosController()
            : this(ServiceResolver.RelatorioRepositoryInstance)
        {
        }

        public RelatoriosController(IRelatorioRepository relatorioRepository)
        {
            _relatorioRepository = relatorioRepository;
        }

        [HttpGet]
        [Route("alunos-por-turma")]
        public IHttpActionResult AlunosPorTurma()
        {
            return Ok(_relatorioRepository.AlunosPorTurma());
        }
    }
}
