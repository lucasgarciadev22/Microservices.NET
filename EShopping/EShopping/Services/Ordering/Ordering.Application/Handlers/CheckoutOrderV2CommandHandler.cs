using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class CheckoutOrderV2CommandHandler(
    IOrderRepository orderRepository,
    IMapper mapper,
    ILogger<CheckoutOrderV2CommandHandler> logger
) : IRequestHandler<CheckoutOrderV2Command, int>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CheckoutOrderV2CommandHandler> _logger = logger;

    public async Task<int> Handle(
        CheckoutOrderV2Command request,
        CancellationToken cancellationToken
    )
    {
        Order orderToAdd = _mapper.Map<Order>(request);
        Order addedOrder = await _orderRepository.AddAsync(orderToAdd);

        _logger.LogInformation(
            "Checkout Order Command Handler(v2): Order {OrderId} successfully created.",
            addedOrder.Id
        );

        return addedOrder.Id;
    }
}
