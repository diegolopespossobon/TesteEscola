# Teste Escola - API de Matriculas

API em ASP.NET Web API sobre .NET Framework 4.8, SQL Server e Dapper.

## Stack

- .NET Framework 4.8
- ASP.NET Web API 2
- SQL Server / LocalDB
- Dapper com SQL escrito manualmente

## Como rodar

1. Abra `TesteEscola.sln` no Visual Studio 2022.
2. Restaure os pacotes NuGet da solucao.
3. Rode o script `TesteEscola.Api/script-banco.sql` no SQL Server.
4. Confira a connection string `TesteEscola` em `TesteEscola.Api/Web.config`.
5. Execute o projeto `TesteEscola.Api` pelo IIS Express.

A connection string padrao usa SQL Server Express:

```xml
Data Source=.\SQLEXPRESS;Initial Catalog=TesteEscola;Integrated Security=True;Encrypt=False;TrustServerCertificate=True
```

## Ajuste feito no script do banco

Adicionei uma constraint de vagas na tabela `Turma` e um indice unico em `Matricula (AlunoId, TurmaId)`.
O indice unico reforca no banco a regra de que o mesmo aluno nao pode ser matriculado duas vezes na mesma turma, inclusive em cenarios concorrentes.

## Endpoints

### Alunos

```http
GET /api/alunos?page=1&pageSize=10&nome=ana
GET /api/alunos/1
POST /api/alunos
PUT /api/alunos/1
DELETE /api/alunos/1
```

Exemplo de `POST`/`PUT`:

```json
{
  "nome": "Marina Costa",
  "email": "marina.costa@email.com",
  "dataNascimento": "2006-04-10"
}
```

`DELETE` faz exclusao logica alterando `Ativo` para `false`.

### Turmas

```http
GET /api/turmas
```

Retorna as turmas com `vagasDisponiveis`. A listagem usa cache em memoria por 5 minutos.

### Matriculas

```http
POST /api/matriculas
```

Exemplo:

```json
{
  "alunoId": 4,
  "turmaId": 2
}
```

Regras aplicadas no service/repository:

- aluno precisa existir e estar ativo
- turma precisa existir
- turma precisa ter vaga disponivel
- aluno nao pode estar matriculado duas vezes na mesma turma
- insercao da matricula e decremento de vaga acontecem na mesma transacao
- cache de turmas e invalidado quando a matricula e criada

### Relatorio

```http
GET /api/relatorios/alunos-por-turma
```

O relatorio e feito diretamente no SQL com `LEFT JOIN` e `GROUP BY`.

## Tela simples

Com a API rodando, acesse:

```text
/Scripts/alunos.html
```

A tela consome `GET /api/alunos` com filtro por nome.

## Testes sugeridos

Com Postman, Insomnia ou curl:

```bash
curl http://localhost:porta/api/alunos
curl http://localhost:porta/api/turmas
curl http://localhost:porta/api/relatorios/alunos-por-turma
curl -X POST http://localhost:porta/api/matriculas -H "Content-Type: application/json" -d "{\"alunoId\":1,\"turmaId\":2}"
```

Casos esperados:

- aluno inativo retorna `409`
- turma lotada retorna `409`
- matricula duplicada retorna `409`
- id inexistente em buscas retorna `404`
- payload invalido retorna `400`

## Observacao sobre Redis

O cache de turmas foi abstraido pela interface `ITurmasCache` e implementado em memoria em `MemoryTurmasCache`.
Para Redis, bastaria criar uma implementacao da mesma interface usando StackExchange.Redis, serializando a lista de turmas em uma chave como `turmas:listagem` e apagando essa chave no metodo `Invalidate`.
