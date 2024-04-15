using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class UpdateOrderCommandHandler(
    IOrderRepository orderRepository,
    IMapper mapper,
    ILogger<UpdateOrderCommandHandler> logger
) : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger = logger;

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        Order orderToUpdate =
            await _orderRepository.GetByIdAsync(request.Id)
            ?? throw new OrderNotFoundException(nameof(Order), request.Id);

        _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
        await _orderRepository.UpdateAsync(orderToUpdate);

        _logger.LogInformation("Order {OrderId} was successfully updated", orderToUpdate.Id);

        return Unit.Value;
    }
}
