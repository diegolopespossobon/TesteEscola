using TesteEscola.Api.Dtos;
using TesteEscola.Api.Models;

namespace TesteEscola.Api.Repositories
{
    public interface IAlunoRepository
    {
        PagedResult<AlunoResponse> Listar(string nome, int page, int pageSize);
        Aluno ObterPorId(int id);
        int Criar(AlunoRequest aluno);
        bool Atualizar(int id, AlunoRequest aluno);
        bool Inativar(int id);
    }
}
