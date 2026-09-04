using TesteEscola.Api.Dtos;

namespace TesteEscola.Api.Repositories
{
    public enum MatriculaStatus
    {
        Sucesso,
        AlunoNaoEncontradoOuInativo,
        TurmaNaoEncontrada,
        TurmaSemVaga,
        MatriculaDuplicada
    }

    public class MatriculaResult
    {
        public MatriculaStatus Status { get; set; }
        public int? Id { get; set; }
    }

    public interface IMatriculaRepository
    {
        MatriculaResult Matricular(MatriculaRequest request);
    }
}
