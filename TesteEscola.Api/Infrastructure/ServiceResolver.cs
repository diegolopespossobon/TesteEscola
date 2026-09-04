using TesteEscola.Api.Data;
using TesteEscola.Api.Repositories;
using TesteEscola.Api.Services;

namespace TesteEscola.Api.Infrastructure
{
    public static class ServiceResolver
    {
        private static readonly IDbConnectionFactory ConnectionFactory = new SqlConnectionFactory();
        private static readonly ITurmasCache TurmasCache = new MemoryTurmasCache();

        private static readonly IAlunoRepository AlunoRepository = new AlunoRepository(ConnectionFactory);
        private static readonly ITurmaRepository TurmaRepository = new TurmaRepository(ConnectionFactory);
        private static readonly IMatriculaRepository MatriculaRepository = new MatriculaRepository(ConnectionFactory);
        private static readonly IRelatorioRepository RelatorioRepository = new RelatorioRepository(ConnectionFactory);

        public static AlunoService AlunoService => new AlunoService(AlunoRepository);
        public static TurmaService TurmaService => new TurmaService(TurmaRepository, TurmasCache);
        public static MatriculaService MatriculaService => new MatriculaService(MatriculaRepository, TurmasCache);
        public static IRelatorioRepository RelatorioRepositoryInstance => RelatorioRepository;
    }
}
