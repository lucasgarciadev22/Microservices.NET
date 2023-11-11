using MediatR;

namespace Discount.Application.Commands;

internal class DeleteDiscountCommand : IRequest<bool>
{
    public string ProductName { get; }

    public DeleteDiscountCommand(string productName)
    {
        ProductName = productName;
    }
}
