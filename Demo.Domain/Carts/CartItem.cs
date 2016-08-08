using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain.Products;

namespace Demo.Domain.Carts
{
    public class CartItem
    {
        public CartItem(Product product, int amount)
        {
            Product = product;
            Amount = amount;
            this.Recalculate();
        }

        public Product Product { get; private set; }

        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public void ChangeAmount(int amount)
        {
            this.Amount += amount;
            this.Recalculate();
        }

        private void Recalculate()
        {
            this.Value = this.Amount*this.Product.Price;
        }
    }
}
