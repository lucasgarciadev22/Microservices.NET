﻿using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Queries;

public class GetDiscountQuery : IRequest<CouponModel>
{
    public string ProductName { get; }

    public GetDiscountQuery(string productName)
    {
        ProductName = productName;
    }
}
