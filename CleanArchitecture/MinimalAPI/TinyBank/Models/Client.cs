using TinyBank.Models.Transaction;

namespace TinyBank.Models;

public record Client(int Id, int Limit, int Balance, IEnumerable<BankTransaction> Transactions);
