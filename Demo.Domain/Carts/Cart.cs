using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Clients;
using Demo.Domain.Shared;
using Demo.Domain.Products;

namespace Demo.Domain.Carts
{
    public class Cart : Entity
    {
        public Cart(Client client)
        {
            Client = client;
            this.Items = new List<CartItem>();
        }

        public Client Client { get; private set; }

        public List<CartItem> Items { get; private set; }

        public decimal Value { get; private set; }

        public void AddToCart(Product product, int amount)
        {
            if(amount < 0)
            {
                throw new InvalidOperationException();
            }

            var item = this.Items.SingleOrDefault(x => x.Product.Id == product.Id);
            if(item == null)
            {
                this.Items.Add(new CartItem(product, amount));
            }
            else
            {
                item.ChangeAmount(amount);
            }

            this.Recalculate();
        }

        public void RemoveFromCart(Product product)
        {
            var item = this.Items.Single(x => x.Product.Id == product.Id);
            this.Items.Remove(item);
            this.Recalculate();
        }

        public void ChangeAmount(Product product, int amount)
        {
            // TODO
        }

        private void Recalculate()
        {
            this.Value = this.Items.Sum(x => x.Value);
        }
    }
}
