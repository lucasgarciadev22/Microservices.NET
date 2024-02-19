using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace TinyBank.Helpers;

public static class DatabaseInitializerHelper
{
    public static void Initialize(BankContext context)
    {
        context.Database.EnsureCreated();

        // Injeção de dados usando SQL raw
        string query =
            @"
            INSERT INTO Clientes (Id, Limite, Saldo)
            VALUES (1, 100000, 0),
                   (2, 80000, 0),
                   (3, 1000000, 0),
                   (4, 10000000, 0),
                   (5, 500000, 0);
        ";

        context.Clients.FromSqlRaw(query);
    }
}
