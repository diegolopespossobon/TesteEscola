using System.Collections.Generic;
using TesteEscola.Api.Dtos;

namespace TesteEscola.Api.Repositories
{
    public interface IRelatorioRepository
    {
        IEnumerable<RelatorioAlunoPorTurmaResponse> AlunosPorTurma();
    }
}
