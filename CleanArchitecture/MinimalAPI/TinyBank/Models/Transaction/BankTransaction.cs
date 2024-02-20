namespace TinyBank.Models.Transaction;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public record BankTransaction
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O valor de Id deve ser um n�mero inteiro positivo.")]
    public int Id { get; init; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O valor de Id deve ser um n�mero inteiro positivo.")]
    public int Value { get; init; }

    [Required]
    [RegularExpression(
        "[cd]",
        ErrorMessage = "O tipo deve ser 'c' para cr�dito ou 'd' para d�bito."
    )]
    public TransactionType Type { get; init; }

    [Required, NotNull]
    [StringLength(10, ErrorMessage = "A descri��o deve ter entre 1 e 10 caracteres.")]
    public string Description { get; init; } = null!;

    [Required]
    public DateTime DoneAt { get; init; }

    [Required, NotNull]
    public Client Client { get; init; } = null!;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O valor de Id deve ser um n�mero inteiro positivo.")]
    public int ClientId { get; init; }
}
