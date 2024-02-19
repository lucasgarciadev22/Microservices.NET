using Microsoft.EntityFrameworkCore;
using TinyBank.Models;

namespace Infrastructure.Context;

public class BankContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
}
