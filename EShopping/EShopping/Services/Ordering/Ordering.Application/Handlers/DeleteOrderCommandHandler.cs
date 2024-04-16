using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger logger)
    : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ILogger _logger = logger;

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        Order orderToDelete =
            await _orderRepository.GetByIdAsync(request.Id)
            ?? throw new OrderNotFoundException(string.Empty, $"{request.Id}");

        await _orderRepository.DeleteAsync(orderToDelete);
        _logger.LogInformation("Order with Id {RequestId} was deleted successfully", request.Id);

        return Unit.Value;
    }
}
