using MediatR;

namespace Ordering.Application.Commands;

public class CheckoutOrderV2Command : IRequest<int>
{
    public string? UserName { get; set; }
    public decimal? TotalPrice { get; set; }
}
