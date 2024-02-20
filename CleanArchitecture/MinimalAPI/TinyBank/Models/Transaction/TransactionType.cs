using System.ComponentModel;

namespace TinyBank.Models.Transaction;

public enum TransactionType
{
    [Description("Credit")]
    c,

    [Description("Debit")]
    d
}
