using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartCommandHandler(IBasketRepository basketRepository)
    : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository = basketRepository;

    public async Task<ShoppingCartResponse> Handle(
        CreateShoppingCartCommand request,
        CancellationToken cancellationToken
    )
    {
        //TODO: Call discount and apply coupons

        ShoppingCart shoppingCart = await _basketRepository.UpdateBasket(
            new ShoppingCart { UserName = request.UserName, Items = request.Items, }
        );

        ShoppingCartResponse shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(
            shoppingCart
        );

        return shoppingCartResponse;
    }
}
