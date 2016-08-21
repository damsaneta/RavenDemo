using Demo.Domain.Products;

namespace Demo.Domain.Orders
{
    public class CartItem
    {
        protected CartItem()
        {
        }

        public CartItem(Product product, int amount)
        {
            ProductId = product.Id;
            Amount = amount;
            this.Recalculate(product);
        }

        public string ProductId { get; private set; }

        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public void ChangeAmount(Product product, int amount)
        {
            this.Amount += amount;
            this.Recalculate(product);
        }

        private void Recalculate(Product product)
        {
            this.Value = this.Amount * product.Price;
        }
    }
}
