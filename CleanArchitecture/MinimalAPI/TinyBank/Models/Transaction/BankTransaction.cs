namespace TinyBank.Models.Transaction;

public record BankTransaction(int Value, TransactionType Type, string Description, DateTime DoneAt);
