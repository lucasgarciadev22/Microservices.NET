using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TinyBank.Models;
using TinyBank.Models.Transaction;

namespace TinyBank.Helpers;

public static class DatabaseHelper
{
    public static bool CanWithdraw(BankTransaction transaction, Client client) =>
        transaction.Type == TransactionType.d && client.Balance - transaction.Value < -client.Limit;

    public static void Initialize(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using BankContext dbContext = scope.ServiceProvider.GetRequiredService<BankContext>();

        dbContext.Database.Migrate();

        // Injeção de dados usando SQL raw
        string query =
            @"
            INSERT INTO clientes (id, limite, saldo)
            VALUES (1, 100000, 0),
                   (2, 80000, 0),
                   (3, 1000000, 0),
                   (4, 10000000, 0),
                   (5, 500000, 0);
        ";

        dbContext.Clients.FromSqlRaw(query);
    }

    public static async Task<int> SaveChangesAsync(this WebApplication app, CancellationToken ct)
    {
        using IServiceScope scope = app.Services.CreateScope();
        using BankContext dbContext = scope.ServiceProvider.GetRequiredService<BankContext>();

        return await dbContext.SaveChangesAsync(ct);
    }

    public static async Task<Client> GetClientAsync(this WebApplication app, int id)
    {
        using IServiceScope scope = app.Services.CreateScope();
        using BankContext dbContext = scope.ServiceProvider.GetRequiredService<BankContext>();

        return await dbContext.Clients
                .FromSqlRaw("SELECT * FROM clientes WHERE id = {0}", id)
                .FirstOrDefaultAsync()
            ?? throw new NullReferenceException("Cliente não encontrado!");
    }

    public static async Task<IEnumerable<BankTransaction>> GetLatestTransactionsAsync(
        this WebApplication app,
        int clientId
    )
    {
        using IServiceScope scope = app.Services.CreateScope();
        using BankContext dbContext = scope.ServiceProvider.GetRequiredService<BankContext>();

        return await dbContext.Transactions
            .FromSqlRaw(
                "SELECT * FROM transacoes WHERE id = {0} ORDER BY realizado_em DESC LIMIT 10",
                clientId
            )
            .ToListAsync();
    }
}
