using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TinyBank.Models.Transaction;

namespace TinyBank.Models;

public sealed record Client
{
    [Key]
    [Column("id")]
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O valor de Id deve ser um n�mero inteiro positivo.")]
    public int Id { get; init; }

    [Column("limite")]
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O Limite deve ser um n�mero inteiro positivo.")]
    public int Limit { get; init; }

    [Column("saldo")]
    [Required]
    [Range(-1000, int.MaxValue, ErrorMessage = "O Saldo m�nimo deve ser -1000.")]
    public int Balance { get; init; }

    public IEnumerable<BankTransaction> Transactions { get; init; } = null!;
}
