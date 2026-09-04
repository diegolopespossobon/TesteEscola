namespace TesteEscola.Api.Dtos
{
    public class RelatorioAlunoPorTurmaResponse
    {
        public int TurmaId { get; set; }
        public string NomeTurma { get; set; }
        public int QuantidadeAlunosMatriculados { get; set; }
        public int VagasRestantes { get; set; }
    }
}
