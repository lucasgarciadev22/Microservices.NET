using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class UpdateOrderCommandHandler(
    IOrderRepository orderRepository,
    IMapper mapper,
    ILogger<UpdateOrderCommandHandler> logger
) : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger = logger;

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate =
            await _orderRepository.GetByIdAsync(request.Id)
            ?? throw new OrderNotFoundException(nameof(Order), request.Id);
    }
}
