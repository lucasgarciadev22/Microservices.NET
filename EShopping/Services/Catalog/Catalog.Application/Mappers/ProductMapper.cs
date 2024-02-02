using AutoMapper;

namespace Catalog.Application.Mappers;

public static class ProductMapper
{
    private static readonly Lazy<IMapper> Lazy =
        new(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p =>
                {
                    return p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                };
                cfg.AddProfile<ProductMappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

    public static IMapper Mapper => Lazy.Value;
}
