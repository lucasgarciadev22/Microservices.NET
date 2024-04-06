using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IMediator mediator, DiscountGrpcService discountGrpcService)
    {
        _mediator = mediator;
        _discountGrpcService = discountGrpcService;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        GetBasketByUserNameQuery query = new(userName);
        ShoppingCartResponse basket = await _mediator.Send(query);
        return Ok(basket);
    }

    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> CreateBasket(
        [FromBody] CreateShoppingCartCommand createShoppingCartCommand
    )
    {
        foreach (Core.Entities.ShoppingCartItem item in createShoppingCartCommand.Items)
        {
            Discount.Grpc.Protos.CouponModel coupon = await _discountGrpcService.GetDiscount(
                item.ProductName
            );
            item.Price -= coupon.Amount;
        }

        ShoppingCartResponse basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);
    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
    {
        DeleteBasketByUserNameCommand query = new(userName);
        await _mediator.Send(query);
        return Ok();
    }
}
