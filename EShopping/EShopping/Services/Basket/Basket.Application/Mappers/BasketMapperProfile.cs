using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;

namespace Basket.Application.Mappers;

public class BasketMapperProfile : Profile
{
    public BasketMapperProfile()
    {
        CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
        CreateMap<ShoppingCartItem, ShoppingCartItemResponse>().ReverseMap();
        CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        CreateMap<BasketCheckoutV2, BasketCheckoutEvent>().ReverseMap();
    }
}
