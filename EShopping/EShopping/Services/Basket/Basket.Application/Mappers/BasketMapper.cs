using AutoMapper;

namespace Basket.Application.Mappers;

public class BasketMapper
{
    private static readonly Lazy<IMapper> Lazy =
        new(() =>
        {
            MapperConfiguration config =
                new(cfg =>
                {
                    cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                    cfg.AddProfile<BasketMapperProfile>();
                });
            IMapper mapper = config.CreateMapper();
            return mapper;
        });

    public static IMapper Mapper => Lazy.Value;
}
