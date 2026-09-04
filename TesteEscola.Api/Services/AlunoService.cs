using TesteEscola.Api.Dtos;
using TesteEscola.Api.Repositories;

namespace TesteEscola.Api.Services
{
    public class AlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public PagedResult<AlunoResponse> Listar(string nome, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            return _alunoRepository.Listar(nome, page, pageSize);
        }

        public AlunoResponse ObterPorId(int id)
        {
            var aluno = _alunoRepository.ObterPorId(id);

            if (aluno == null)
            {
                return null;
            }

            return new AlunoResponse
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                DataNascimento = aluno.DataNascimento,
                Ativo = aluno.Ativo,
                DataCadastro = aluno.DataCadastro
            };
        }

        public int Criar(AlunoRequest aluno)
        {
            return _alunoRepository.Criar(aluno);
        }

        public bool Atualizar(int id, AlunoRequest aluno)
        {
            return _alunoRepository.Atualizar(id, aluno);
        }

        public bool Inativar(int id)
        {
            return _alunoRepository.Inativar(id);
        }
    }
}
