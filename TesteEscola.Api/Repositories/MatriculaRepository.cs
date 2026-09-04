using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using TesteEscola.Api.Data;
using TesteEscola.Api.Dtos;

namespace TesteEscola.Api.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MatriculaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public MatriculaResult Matricular(MatriculaRequest request)
        {
            using (var connection = _connectionFactory.Create())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var alunoAtivo = connection.QuerySingleOrDefault<bool?>(
                            @"SELECT Ativo FROM dbo.Aluno WITH (UPDLOCK, HOLDLOCK) WHERE Id = @AlunoId;",
                            request,
                            transaction);

                        if (alunoAtivo != true)
                        {
                            transaction.Rollback();
                            return new MatriculaResult { Status = MatriculaStatus.AlunoNaoEncontradoOuInativo };
                        }

                        var turmaExiste = connection.QuerySingleOrDefault<int?>(
                            @"SELECT Id FROM dbo.Turma WITH (UPDLOCK, HOLDLOCK) WHERE Id = @TurmaId;",
                            request,
                            transaction);

                        if (!turmaExiste.HasValue)
                        {
                            transaction.Rollback();
                            return new MatriculaResult { Status = MatriculaStatus.TurmaNaoEncontrada };
                        }

                        var jaMatriculado = connection.ExecuteScalar<int>(
                            @"SELECT COUNT(1) FROM dbo.Matricula WITH (UPDLOCK, HOLDLOCK) WHERE AlunoId = @AlunoId AND TurmaId = @TurmaId;",
                            request,
                            transaction) > 0;

                        if (jaMatriculado)
                        {
                            transaction.Rollback();
                            return new MatriculaResult { Status = MatriculaStatus.MatriculaDuplicada };
                        }

                        var vagasAtualizadas = connection.Execute(
                            @"UPDATE dbo.Turma
                              SET VagasDisponiveis = VagasDisponiveis - 1
                              WHERE Id = @TurmaId AND VagasDisponiveis > 0;",
                            request,
                            transaction);

                        if (vagasAtualizadas == 0)
                        {
                            transaction.Rollback();
                            return new MatriculaResult { Status = MatriculaStatus.TurmaSemVaga };
                        }

                        var matriculaId = connection.QuerySingle<int>(
                            @"INSERT INTO dbo.Matricula (AlunoId, TurmaId)
                              VALUES (@AlunoId, @TurmaId);
                              SELECT CAST(SCOPE_IDENTITY() AS INT);",
                            request,
                            transaction);

                        transaction.Commit();
                        return new MatriculaResult { Status = MatriculaStatus.Sucesso, Id = matriculaId };
                    }
                    catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
                    {
                        transaction.Rollback();
                        return new MatriculaResult { Status = MatriculaStatus.MatriculaDuplicada };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
