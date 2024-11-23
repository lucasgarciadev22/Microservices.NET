using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer;

public class BasketOrderingV2Consumer(
    IMediator mediator,
    IMapper mapper,
    ILogger<BasketOrderingV2Consumer> logger
) : IConsumer<BasketCheckoutV2Event>
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<BasketOrderingV2Consumer> _logger = logger;

    public async Task Consume(ConsumeContext<BasketCheckoutV2Event> context)
    {
        using IDisposable? scope = _logger.BeginScope(
            "Consuming Basket Checkout Event (v2) for {CorrelationId}",
            context.Message.CorrelationId
        );
        CheckoutOrderV2Command cmd = _mapper.Map<CheckoutOrderV2Command>(context.Message);
        int result = await _mediator.Send(cmd);
        _logger.LogInformation(
            "Basket Checkout Event (v2) consumed successfully. Created Order Id: {OrderId}",
            result
        );
    }
}
