using System.Collections.Generic;
using System.Linq;
using Dapper;
using TesteEscola.Api.Data;
using TesteEscola.Api.Dtos;

namespace TesteEscola.Api.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RelatorioRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<RelatorioAlunoPorTurmaResponse> AlunosPorTurma()
        {
            const string sql = @"SELECT t.Id AS TurmaId,
                                        t.Nome AS NomeTurma,
                                        COUNT(m.Id) AS QuantidadeAlunosMatriculados,
                                        t.VagasDisponiveis AS VagasRestantes
                                   FROM dbo.Turma t
                              LEFT JOIN dbo.Matricula m ON m.TurmaId = t.Id
                               GROUP BY t.Id, t.Nome, t.VagasDisponiveis
                               ORDER BY t.Nome;";

            using (var connection = _connectionFactory.Create())
            {
                return connection.Query<RelatorioAlunoPorTurmaResponse>(sql).ToList();
            }
        }
    }
}
