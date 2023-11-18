using MediatR;

namespace Discount.Application.Commands;

public class DeleteDiscountCommand : IRequest<bool>
{
    public string ProductName { get; }

    public DeleteDiscountCommand(string productName)
    {
        ProductName = productName;
    }
}
