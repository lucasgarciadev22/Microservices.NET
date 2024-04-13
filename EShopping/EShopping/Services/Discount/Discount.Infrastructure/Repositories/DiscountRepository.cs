using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using NpgsqlConnection connection =
            new(_configuration.GetValue<string>("DatabaseSettings:ConnectionString")); //get the connection string from JSON

        int affected = await connection.ExecuteAsync(
            "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName,@Description,@Amount",
            new
            {
                coupon.ProductName,
                coupon.Description,
                coupon.Amount
            }
        ); //creates the register and returns the affected row from db

        if (affected is 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using NpgsqlConnection connection =
            new(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        int affected = await connection.ExecuteAsync(
            "DELETE FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName }
        ); //deletes the register and returns the affected row from db

        if (affected is 0)
            return false;

        return true;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        await using NpgsqlConnection connection =
            new(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        Coupon coupon =
            await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName }
            )
            ?? new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Available"
            }; //send query params based on args

        return coupon;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using NpgsqlConnection connection =
            new(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        int affected = await connection.ExecuteAsync(
            "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
            new
            {
                coupon.ProductName,
                coupon.Description,
                coupon.Amount,
                coupon.Id
            }
        ); //updates the register and returns the affected row from db

        if (affected is 0)
            return false;

        return true;
    }
}
