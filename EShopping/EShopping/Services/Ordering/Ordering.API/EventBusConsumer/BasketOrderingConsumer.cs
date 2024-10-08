﻿using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer;

public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<BasketOrderingConsumer> _logger;

    public BasketOrderingConsumer(
        IMediator mediator,
        IMapper mapper,
        ILogger<BasketOrderingConsumer> logger
    )
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        //create consume command based on checkout event received
        CheckoutOrderCommand command = _mapper.Map<CheckoutOrderCommand>(context.Message);
        int result = await _mediator.Send(command);
        _logger.LogInformation(
            $"BasketCheckoutEvent consumed successfully. Created Order Id: {result}"
        );
    }
}
