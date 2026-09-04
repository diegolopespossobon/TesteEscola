using System.Web.Http;
using TesteEscola.Api.Infrastructure;
using TesteEscola.Api.Services;

namespace TesteEscola.Api.Controllers
{
    [RoutePrefix("api/turmas")]
    public class TurmasController : ApiController
    {
        private readonly TurmaService _turmaService;

        public TurmasController()
            : this(ServiceResolver.TurmaService)
        {
        }

        public TurmasController(TurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_turmaService.ListarComVagas());
        }
    }
}
