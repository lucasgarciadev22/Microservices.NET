namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }
        public IList<ShoppingCartItemResponse> Items { get; set; }

        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }

        public ShoppingCartResponse()
        {

        }

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * item.Quantity;
                }

                return total;
            }
        }
    }
}
