using System.Linq;
using Dapper;
using TesteEscola.Api.Data;
using TesteEscola.Api.Dtos;
using TesteEscola.Api.Models;

namespace TesteEscola.Api.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AlunoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public PagedResult<AlunoResponse> Listar(string nome, int page, int pageSize)
        {
            const string sql = @"SELECT COUNT(1)
                                   FROM dbo.Aluno
                                  WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%');

                                 SELECT Id, Nome, Email, DataNascimento, Ativo, DataCadastro
                                   FROM dbo.Aluno
                                  WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')
                               ORDER BY Nome
                              OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            using (var connection = _connectionFactory.Create())
            using (var multi = connection.QueryMultiple(sql, new
            {
                Nome = string.IsNullOrWhiteSpace(nome) ? null : nome.Trim(),
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            }))
            {
                return new PagedResult<AlunoResponse>
                {
                    Page = page,
                    PageSize = pageSize,
                    Total = multi.Read<int>().Single(),
                    Items = multi.Read<AlunoResponse>().ToList()
                };
            }
        }

        public Aluno ObterPorId(int id)
        {
            const string sql = @"SELECT Id, Nome, Email, DataNascimento, Ativo, DataCadastro
                                   FROM dbo.Aluno
                                  WHERE Id = @Id;";

            using (var connection = _connectionFactory.Create())
            {
                return connection.QuerySingleOrDefault<Aluno>(sql, new { Id = id });
            }
        }

        public int Criar(AlunoRequest aluno)
        {
            const string sql = @"INSERT INTO dbo.Aluno (Nome, Email, DataNascimento, Ativo)
                                      VALUES (@Nome, @Email, @DataNascimento, 1);

                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var connection = _connectionFactory.Create())
            {
                return connection.QuerySingle<int>(sql, aluno);
            }
        }

        public bool Atualizar(int id, AlunoRequest aluno)
        {
            const string sql = @"UPDATE dbo.Aluno
                                    SET Nome = @Nome,
                                        Email = @Email,
                                        DataNascimento = @DataNascimento
                                  WHERE Id = @Id;";

            using (var connection = _connectionFactory.Create())
            {
                return connection.Execute(sql, new
                {
                    Id = id,
                    aluno.Nome,
                    aluno.Email,
                    aluno.DataNascimento
                }) > 0;
            }
        }

        public bool Inativar(int id)
        {
            const string sql = @"UPDATE dbo.Aluno
                                    SET Ativo = 0
                                  WHERE Id = @Id;";

            using (var connection = _connectionFactory.Create())
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }
    }
}
