using Demo.Domain.Products;

namespace Demo.Domain.Orders
{
    public class OrderItem
    {
        public OrderItem(CartItem cartItem, Product product)
        {
            this.Product = product;
            this.Amount = cartItem.Amount;
            this.Value = cartItem.Value;
        }

        public Product Product { get; private set; }

        public int Amount { get; private set; }

        public decimal Value { get; private set; }
    }
}
