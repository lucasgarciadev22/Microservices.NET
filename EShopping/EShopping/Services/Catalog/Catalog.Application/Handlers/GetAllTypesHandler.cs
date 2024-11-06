using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllTypesHandler(ITypesRepository typesRepository)
    : IRequestHandler<GetAllTypesQuery, IList<TypeResponse>>
{
    private readonly ITypesRepository _typesRepository = typesRepository;

    public async Task<IList<TypeResponse>> Handle(
        GetAllTypesQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<ProductType> typesList = await _typesRepository.GetAllTypes();
        IList<TypeResponse> typesResponseList = ProductMapper.Mapper.Map<IList<TypeResponse>>(
            typesList
        );
        return typesResponseList;
    }
}
