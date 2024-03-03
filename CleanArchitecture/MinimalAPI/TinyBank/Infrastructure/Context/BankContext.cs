using Microsoft.EntityFrameworkCore;
using TinyBank.Models;
using TinyBank.Models.Transaction;

namespace Infrastructure.Context;

public class BankContext(DbContextOptions<BankContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<BankTransaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().ToTable("clientes");
        modelBuilder.Entity<BankTransaction>().ToTable("transacoes");
    }
}
