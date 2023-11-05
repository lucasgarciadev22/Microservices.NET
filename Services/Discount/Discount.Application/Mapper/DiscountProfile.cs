using AutoMapper;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mapper
{
    internal class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
