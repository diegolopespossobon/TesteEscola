using System.ComponentModel.DataAnnotations;

namespace TesteEscola.Api.Dtos
{
    public class MatriculaRequest
    {
        [Range(1, int.MaxValue)]
        public int AlunoId { get; set; }

        [Range(1, int.MaxValue)]
        public int TurmaId { get; set; }
    }
}
