using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class DeleteOrderCommandHandler(
    IOrderRepository orderRepository,
    ILogger<DeleteOrderCommandHandler> logger
) : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger = logger;

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        Order orderToDelete =
            await _orderRepository.GetByIdAsync(request.Id)
            ?? throw new OrderNotFoundException(string.Empty, $"{request.Id}");

        await _orderRepository.DeleteAsync(orderToDelete);
        _logger.LogInformation("Order {OrderId} was successfully deleted", request.Id);

        return Unit.Value;
    }
}
