using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartCommandHandler(
    IBasketRepository basketRepository,
    DiscountGrpcService discountGrpcService
) : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository = basketRepository;
    private readonly DiscountGrpcService _discountGrpcService = discountGrpcService;

    public async Task<ShoppingCartResponse> Handle(
        CreateShoppingCartCommand request,
        CancellationToken cancellationToken
    )
    {
        foreach (ShoppingCartItem item in request.Items)
        {
            CouponModel coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

        ShoppingCart shoppingCart = await _basketRepository.UpdateBasket(
            new ShoppingCart { UserName = request.UserName, Items = request.Items, }
        );

        ShoppingCartResponse shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(
            shoppingCart
        );

        return shoppingCartResponse;
    }
}
