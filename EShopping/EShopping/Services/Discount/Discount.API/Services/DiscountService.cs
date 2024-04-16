using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services;

public class DiscountService(IMediator mediator, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<DiscountService> _logger = logger;

    public override async Task<CouponModel> GetDiscount(
        GetDiscountRequest request,
        ServerCallContext context
    )
    {
        GetDiscountQuery query = new(request.ProductName);
        CouponModel result = await _mediator.Send(query);

        _logger.LogInformation(
            "Discount was found for the product name: {ProductName} and the amount is: {Amount}",
            request.ProductName,
            result.Amount
        );

        return result;
    }

    public override async Task<CouponModel> CreateDiscount(
        CreateDiscountRequest request,
        ServerCallContext context
    )
    {
        CreateDiscountCommand cmd =
            new()
            {
                Amount = request.Coupon.Amount,
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
            };

        CouponModel result = await _mediator.Send(cmd);

        _logger.LogInformation(
            "Discount was successfully created for product: {ProductName}",
            result.ProductName
        );

        return result;
    }

    public override async Task<CouponModel> UpdateDiscount(
        UpdateDiscountRequest request,
        ServerCallContext context
    )
    {
        UpdateDiscountCommand cmd =
            new()
            {
                Id = request.Coupon.Id,
                Amount = request.Coupon.Amount,
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
            };

        CouponModel result = await _mediator.Send(cmd);

        _logger.LogInformation(
            "Discount was successfully updated for product: {ProductName}",
            result.ProductName
        );

        return result;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(
        DeleteDiscountRequest request,
        ServerCallContext context
    )
    {
        DeleteDiscountCommand cmd = new(request.ProductName);
        bool deleted = await _mediator.Send(cmd);
        DeleteDiscountResponse response = new() { Success = deleted };

        return response;
    }
}
