using TinyBank.Models;
using TinyBank.Models.Transaction;

namespace TinyBank.Helpers;

public static class ApiMapperHelper
{
    public static void MapMinimalEndpoints(this WebApplication app)
    {
        app.MapPost(
            "/clientes/{id}/transacoes",
            async (int id, BankTransaction transaction, CancellationToken ct) =>
            {
                Client? client = await app.GetClientAsync(id);

                if (client == null)
                    return Results.NotFound();

                if (DatabaseHelper.CanWithdraw(transaction, client))
                    return Results.UnprocessableEntity("Transação inválida");

                if (transaction.Type == TransactionType.c)
                    client = client with { Balance = client.Limit - transaction.Value };
                else
                    client = client with { Balance = client.Balance - transaction.Value };

                await app.SaveChangesAsync(ct);

                return Results.Ok(new TransactionResult(client.Limit, client.Balance));
            }
        );

        app.MapGet(
            "/clientes/{id}/extrato",
            async (int id, CancellationToken ct) =>
            {
                Client? client = await app.GetClientAsync(id);

                if (client == null)
                    return Results.NotFound();

                IEnumerable<BankTransaction> lastTransactions =
                    await app.GetLatestTransactionsAsync(client.Id);

                Client response = client with { Transactions = lastTransactions };

                return Results.Ok(response);
            }
        );
    }
}
