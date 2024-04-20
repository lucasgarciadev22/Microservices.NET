using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

public class BasketController(IMediator mediator, IPublishEndpoint publishEndpoint) : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasket")]
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
        ShoppingCartResponse basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);
    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasket")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
    {
        DeleteBasketByUserNameCommand basket = new(userName);
        await _mediator.Send(basket);
        return Ok();
    }

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        GetBasketByUserNameQuery query = new(basketCheckout.UserName);
        ShoppingCartResponse basket = await _mediator.Send(query);

        if (basket == null)
            return BadRequest();

        BasketCheckoutEvent eventMessage = BasketMapper.Mapper.Map<BasketCheckoutEvent>(
            basketCheckout
        );
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage);

        DeleteBasketByUserNameCommand deleteBasket = new(basketCheckout.UserName);
        await _mediator.Send(deleteBasket);

        return Ok(basket);
    }
}
