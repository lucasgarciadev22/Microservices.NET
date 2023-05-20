using AutoMapper;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
  public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
  {
    private readonly IBrandRepository _brandRepository;
    public GetAllBrandsHandler(IBrandRepository brandRepository, IMapper mapper)
    {
      //Query hanlder based on the query response model (BrandsQuery)
      _brandRepository = brandRepository;
    }
    public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
      //Generates a map based on the expected response (BrandResponse) using Product specific Mapper;
      var brandList = await _brandRepository.GetAllBrands();
      var brandResponseList = ProductMapper.CurrentMap.Map<IList<BrandResponse>>(brandList).ToList();

      return brandResponseList;
    }
  }
}