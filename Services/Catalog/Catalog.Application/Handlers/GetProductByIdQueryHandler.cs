using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
  public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
  {
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }
    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetProduct(request.Id);
      //returns a single product response 
      var productResponse = ProductMapper.CurrentMap.Map<ProductResponse>(product);

      return productResponse;
    }
  }
}