using System.Collections.Generic;
using System.Linq;
using Dapper;
using TesteEscola.Api.Data;
using TesteEscola.Api.Dtos;

namespace TesteEscola.Api.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TurmaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<TurmaResponse> ListarComVagas()
        {
            const string sql = @"
SELECT Id, Nome, Periodo, VagasTotal, VagasDisponiveis
FROM dbo.Turma
ORDER BY Nome;";

            using (var connection = _connectionFactory.Create())
            {
                return connection.Query<TurmaResponse>(sql).ToList();
            }
        }
    }
}
