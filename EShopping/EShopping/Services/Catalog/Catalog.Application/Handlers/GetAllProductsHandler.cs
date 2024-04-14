using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class GetAllProductsHandler(
    IProductRepository productRepository,
    ILogger<GetAllProductsHandler> logger
) : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ILogger<GetAllProductsHandler> _logger = logger;

    public async Task<Pagination<ProductResponse>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        Pagination<Product> productList = await _productRepository.GetProducts(
            request.CatalogSpecParams
        );
        Pagination<ProductResponse> productResponseList = ProductMapper.Mapper.Map<
            Pagination<ProductResponse>
        >(productList);
        _logger.LogDebug(
            "Received Product List.Total Count: {productList}",
            productResponseList.Count
        );
        return productResponseList;
    }
}
