using AutoMapper;

namespace Catalog.Application.Mappers
{
  public static class ProductMapper
  {
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.ShouldMapProperty = prop => prop.GetMethod.IsPublic || prop.GetMethod.IsAssembly;
        cfg.AddProfile<ProductMappingProfile>();
      });

      var mapper = config.CreateMapper();
      return mapper;
    });

    public static IMapper CurrentMap => Lazy.Value; //gets the current lazy map instance
  }
}