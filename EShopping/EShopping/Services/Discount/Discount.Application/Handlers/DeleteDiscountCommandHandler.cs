using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers;

public class DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
    : IRequestHandler<DeleteDiscountCommand, bool>
{
    private readonly IDiscountRepository _discountRepository = discountRepository;

    public async Task<bool> Handle(
        DeleteDiscountCommand request,
        CancellationToken cancellationToken
    )
    {
        return await _discountRepository.DeleteDiscount(request.ProductName);
    }
}
