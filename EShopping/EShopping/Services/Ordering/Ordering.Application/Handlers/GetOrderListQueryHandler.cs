using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<OrderResponse>> Handle(
        GetOrderListQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<Core.Entities.Order> orderList = await _orderRepository.GetOrdersByUserName(
            request.UserName
        );

        return _mapper.Map<List<OrderResponse>>(orderList);
    }
}
