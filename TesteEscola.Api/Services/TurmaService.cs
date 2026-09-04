using System.Collections.Generic;
using TesteEscola.Api.Dtos;
using TesteEscola.Api.Infrastructure;
using TesteEscola.Api.Repositories;

namespace TesteEscola.Api.Services
{
    public class TurmaService
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly ITurmasCache _cache;

        public TurmaService(ITurmaRepository turmaRepository, ITurmasCache cache)
        {
            _turmaRepository = turmaRepository;
            _cache = cache;
        }

        public IEnumerable<TurmaResponse> ListarComVagas()
        {
            var cached = _cache.Get();

            if (cached != null)
            {
                return cached;
            }

            var turmas = _turmaRepository.ListarComVagas();
            _cache.Set(turmas);
            return turmas;
        }
    }
}
