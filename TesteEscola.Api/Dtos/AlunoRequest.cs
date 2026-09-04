using System;
using System.ComponentModel.DataAnnotations;

namespace TesteEscola.Api.Dtos
{
    public class AlunoRequest
    {
        [Required]
        [StringLength(120)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string Email { get; set; }

        [Required]
        public DateTime? DataNascimento { get; set; }
    }
}
