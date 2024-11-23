using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2;

[ApiVersion("2")]
public class BasketController : ApiController
{
    public readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<BasketController> _logger;

    public BasketController(
        IMediator mediator,
        IPublishEndpoint publishEndpoint,
        ILogger<BasketController> logger
    )
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
    {
        //Get the existing basket with username
        GetBasketByUserNameQuery query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        Application.Responses.ShoppingCartResponse basket = await _mediator.Send(query);
        if (basket == null)
        {
            return BadRequest();
        }

        BasketCheckoutEventV2 eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEventV2>(
            basketCheckout
        );
        eventMsg.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMsg);
        _logger.LogInformation($"Basket Published for {basket.UserName} with V2 endpoint");
        //remove the basket
        DeleteBasketByUserNameCommand deleteCmd = new DeleteBasketByUserNameCommand(
            basketCheckout.UserName
        );
        await _mediator.Send(deleteCmd);
        return Accepted();
    }
}
