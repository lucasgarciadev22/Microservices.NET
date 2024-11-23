using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer;

public class BasketOrderingConsumer(
    IMediator mediator,
    IMapper mapper,
    ILogger<BasketOrderingConsumer> logger
) : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<BasketOrderingConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        using IDisposable? scope = _logger.BeginScope(
            "Consuming Basket Checkout Event (v1) for {CorrelationId}",
            context.Message.CorrelationId
        );

        //create consume command based on checkout event received
        CheckoutOrderCommand command = _mapper.Map<CheckoutOrderCommand>(context.Message);
        int result = await _mediator.Send(command);
        _logger.LogInformation(
            "Basket Checkout Event (v1) consumed successfully. Created Order Id: {OrderId}",
            result
        );
    }
}
