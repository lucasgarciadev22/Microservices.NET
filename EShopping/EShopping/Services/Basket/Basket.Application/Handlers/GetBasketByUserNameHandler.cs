using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class GetBasketByUserNameHandler(IBasketRepository basketRepository)
    : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<ShoppingCartResponse> Handle(
        GetBasketByUserNameQuery request,
        CancellationToken cancellationToken
    )
    {
        ShoppingCart shoppingCart = await _basketRepository.GetBasket(request.UserName);
        ShoppingCartResponse shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(
            shoppingCart
        );

        return shoppingCartResponse;
    }
}
