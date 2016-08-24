using Demo.Domain.Products;

namespace Demo.Domain.Orders
{
    public class OrderItem
    {
        public OrderItem(int amount, decimal value, Product product)
        {
            this.Product = product;
            this.Amount = amount;
            this.Value = value;
        }

        public Product Product { get; private set; }

        public int Amount { get; private set; }

        public decimal Value { get; private set; }
    }
}
