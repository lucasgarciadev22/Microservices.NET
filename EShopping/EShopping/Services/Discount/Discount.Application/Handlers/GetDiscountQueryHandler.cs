﻿using Discount.Application.Queries;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<CouponModel> Handle(
        GetDiscountQuery request,
        CancellationToken cancellationToken
    )
    {
        Coupon coupon = await _discountRepository.GetDiscount(request.ProductName);

        if (coupon == null)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    $"Discount with the product name: {request.ProductName} was not found"
                )
            );
        }
        //can use automapper here as well... as seen in create command example
        CouponModel model =
            new()
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                Amount = coupon.Amount,
                Description = coupon.Description,
            };

        return model;
    }
}
