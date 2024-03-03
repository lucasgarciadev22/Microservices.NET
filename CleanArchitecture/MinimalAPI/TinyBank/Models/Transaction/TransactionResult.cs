using System.Text.Json.Serialization;

namespace TinyBank.Models.Transaction;

public sealed record TransactionResult
{
    [JsonPropertyName("limite")]
    public int Limit { get; init; }

    [JsonPropertyName("saldo")]
    public int Balance { get; init; }

    public TransactionResult(int limit, int balance)
    {
        Limit = limit;
        Balance = balance;
    }
}
