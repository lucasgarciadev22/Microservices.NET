using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class CheckoutOrderCommandHandler(
    IOrderRepository orderRepository,
    IMapper mapper,
    ILogger<CheckoutOrderCommandHandler> logger
) : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger = logger;

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        Order orderToAdd = _mapper.Map<Order>(request);
        Order addedOrder = await _orderRepository.AddAsync(orderToAdd);

        _logger.LogInformation("Order {GeneratedOrder} successfully created.", addedOrder);

        return addedOrder.Id;
    }
}
