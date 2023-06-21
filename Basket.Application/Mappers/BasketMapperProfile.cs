using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers
{
    public class BasketMapperProfile : Profile
    {
        public BasketMapperProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
        }
    }
}
