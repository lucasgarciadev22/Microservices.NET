using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository(IDistributedCache redisCache) : IBasketRepository
{
    private readonly IDistributedCache _redisCache = redisCache;

    public async Task DeleteBasket(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }

    public async Task<ShoppingCart> GetBasket(string userName)
    {
        string? basket = await _redisCache.GetStringAsync(userName);

        return JsonSerializer.Deserialize<ShoppingCart>(basket) ?? new ShoppingCart();
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        await _redisCache.SetStringAsync(
            shoppingCart.UserName,
            JsonSerializer.Serialize(shoppingCart)
        );

        return await GetBasket(shoppingCart.UserName);
    }
}
