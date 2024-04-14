using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateProductHandler(IProductRepository productRepository)
    : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ProductResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        Product productEntity =
            ProductMapper.Mapper.Map<Product>(request)
            ?? throw new ApplicationException(
                "There is an issue with mapping while creating new product"
            );
        Product newProduct = await _productRepository.CreateProduct(productEntity);
        ProductResponse productResponse = ProductMapper.Mapper.Map<ProductResponse>(newProduct);
        return productResponse;
    }
}
