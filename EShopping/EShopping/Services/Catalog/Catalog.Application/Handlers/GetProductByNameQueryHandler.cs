using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductByNameQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByNameQuery, IList<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IList<ProductResponse>> Handle(
        GetProductByNameQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<Product> productList = await _productRepository.GetProductByName(request.Name);
        IList<ProductResponse> productResponseList = ProductMapper.Mapper.Map<
            IList<ProductResponse>
        >(productList);
        return productResponseList;
    }
}
