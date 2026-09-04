using TesteEscola.Api.Dtos;
using TesteEscola.Api.Infrastructure;
using TesteEscola.Api.Repositories;

namespace TesteEscola.Api.Services
{
    public class MatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ITurmasCache _turmasCache;

        public MatriculaService(IMatriculaRepository matriculaRepository, ITurmasCache turmasCache)
        {
            _matriculaRepository = matriculaRepository;
            _turmasCache = turmasCache;
        }

        public MatriculaResult Matricular(MatriculaRequest request)
        {
            var result = _matriculaRepository.Matricular(request);

            if (result.Status == MatriculaStatus.Sucesso)
            {
                _turmasCache.Invalidate();
            }

            return result;
        }
    }
}
