using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class CreateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
    : IRequestHandler<CreateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _discountRepository = discountRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CouponModel> Handle(
        CreateDiscountCommand request,
        CancellationToken cancellationToken
    )
    {
        Coupon coupon = _mapper.Map<Coupon>(request);

        bool created = await _discountRepository.CreateDiscount(coupon);

        if (created)
        {
            CouponModel model = _mapper.Map<CouponModel>(coupon);
            return model;
        }

        throw new RpcException(
            new Status(
                StatusCode.NotFound,
                $"Discount with the product name: {request.ProductName} could not be created"
            )
        );
    }
}
