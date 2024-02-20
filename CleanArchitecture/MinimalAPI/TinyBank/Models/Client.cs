using System.ComponentModel.DataAnnotations;
using TinyBank.Models.Transaction;

namespace TinyBank.Models;

public record Client
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O valor de Id deve ser um n�mero inteiro positivo.")]
    public int Id { get; init; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O Limite deve ser um n�mero inteiro positivo.")]
    public int Limit { get; init; }

    [Required]
    [Range(-1000, int.MaxValue, ErrorMessage = "O Saldo m�nimo deve ser -1000.")]
    public int Balance { get; init; }

    public IEnumerable<BankTransaction> Transactions { get; init; } = null!;
}
