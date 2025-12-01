using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIUsuarios.Domain.Entities;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;

    [Required]
    public DateTime DataNascimento { get; set; }

    [Phone]
    public string? Telefone { get; set; }

    [Required]
    public bool Ativo { get; set; } = true;

    [Required]
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public DateTime? DataAtualizacao { get; set; }
}
