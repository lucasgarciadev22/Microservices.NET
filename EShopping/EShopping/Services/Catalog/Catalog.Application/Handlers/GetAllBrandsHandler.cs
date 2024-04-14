using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllBrandsHandler(IBrandRepository brandRepository)
    : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
{
    private readonly IBrandRepository _brandRepository = brandRepository;

    public async Task<IList<BrandResponse>> Handle(
        GetAllBrandsQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<ProductBrand> brandList = await _brandRepository.GetAllBrands();
        IList<BrandResponse> brandResponseList = ProductMapper.Mapper.Map<
            IList<ProductBrand>,
            IList<BrandResponse>
        >(brandList.ToList());
        return brandResponseList;
    }
}
