using MediatR;
using Ordering.Application.Commands;

namespace Ordering.Application.Handlers;

internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    public Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
