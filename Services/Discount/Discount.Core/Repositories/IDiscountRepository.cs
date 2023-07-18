using Discount.Core.Entities;

namespace Discount.Core.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string poductName);
        Task<Coupon> CreateDiscount(Coupon coupon);
        Task<Coupon> GetDiscount(Coupon coupon);
        Task<Coupon> DeleteDiscount(string poductName);
    }
}
