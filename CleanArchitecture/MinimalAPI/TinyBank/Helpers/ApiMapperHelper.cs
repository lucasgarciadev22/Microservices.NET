using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TinyBank.Models.Transaction;

namespace TinyBank.Helpers;

public static class ApiMapperHelper
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost(
            "/clientes/{id}/transacoes",
            (int id, BankTransaction transaction) =>
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BankContext>();

                var client = dbContext.Clients.FirstOrDefault(c => c.Id == id);

                if (client == null)
                    return Results.NotFound();

                if (
                    transaction.Type == TransactionType.Debit
                    && client.Balance - transaction.Value < -client.Limit
                )
                    return Results.UnprocessableEntity("Transação inválida");

                if (transaction.Type == TransactionType.Credit)
                    client = client with { Balance = client.Balance + transaction.Value };
                else
                    client = client with { Balance = client.Balance - transaction.Value };

                dbContext.SaveChanges();

                return Results.Ok(new { limit = client.Limit, balance = client.Balance });
            }
        );

        app.MapGet(
            "/clientes/{id}/extrato",
            (int id) =>
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BankContext>();

                var cliente = dbContext
                    .Clients.Include(c => c.Transactions)
                    .FirstOrDefault(c => c.Id == id);

                if (cliente == null)
                    return Results.NotFound();

                var ultimasTransacoes = cliente
                    .Transactions.OrderByDescending(t => t.DoneAt)
                    .Take(10)
                    .ToList();

                var response = new
                {
                    saldoTotal = cliente.Balance,
                    data_extrato = DateTime.UtcNow,
                    limite = cliente.Limit,
                    ultimas_transacoes = ultimasTransacoes.Select(t => new
                    {
                        valor = t.Value,
                        tipo = t.Type.ToString().ToLower(),
                        descricao = t.Description,
                        realizada_em = t.DoneAt
                    })
                };

                return Results.Ok(response);
            }
        );
    }
}
