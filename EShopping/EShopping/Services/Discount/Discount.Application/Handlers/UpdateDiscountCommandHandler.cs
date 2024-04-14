using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class UpdateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
    : IRequestHandler<UpdateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _discountRepository = discountRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CouponModel> Handle(
        UpdateDiscountCommand request,
        CancellationToken cancellationToken
    )
    {
        Coupon coupon = _mapper.Map<Coupon>(request);

        bool updated = await _discountRepository.UpdateDiscount(coupon);

        if (updated)
        {
            CouponModel model = _mapper.Map<CouponModel>(coupon);
            return model;
        }

        throw new RpcException(
            new Status(
                StatusCode.NotFound,
                $"Discount with the product name: {request.ProductName} could not be updated"
            )
        );
    }
}
