using Microsoft.EntityFrameworkCore;
using TinyBank.Models;
using TinyBank.Models.Transaction;

namespace Infrastructure.Context;

public class BankContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<BankTransaction> Transactions { get; set; }
}
