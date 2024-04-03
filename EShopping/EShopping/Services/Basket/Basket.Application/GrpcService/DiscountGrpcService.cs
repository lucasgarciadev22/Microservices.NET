using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _serviceClient;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient serviceClient)
    {
        _serviceClient = serviceClient;
    }

    private async Task<CouponModel> GetDiscount(string productName)
    {
        GetDiscountRequest discountRequest = new() { ProductName = productName };
        return await _serviceClient.GetDiscountAsync(discountRequest);
    }
}
