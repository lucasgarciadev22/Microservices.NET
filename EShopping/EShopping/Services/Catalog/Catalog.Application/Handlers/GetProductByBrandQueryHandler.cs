using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductByBrandQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByBrandQuery, IList<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IList<ProductResponse>> Handle(
        GetProductByBrandQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<Product> productList = await _productRepository.GetProductByBrand(
            request.Brandname
        );
        IList<ProductResponse> productResponseList = ProductMapper.Mapper.Map<
            IList<ProductResponse>
        >(productList);
        return productResponseList;
    }
}
