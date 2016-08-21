using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Products;
using Demo.Domain.Shared;
using Demo.Domain.Users;

namespace Demo.Domain.Orders
{
    public class Cart : Entity
    {
        protected Cart()
        {
        }

        public Cart(Client client)
        {
            this.ClientId = client.Id;
            this.Items = new List<CartItem>();
        }

        public string ClientId { get; private set; }

        public List<CartItem> Items { get; private set; }

        public decimal Value { get; private set; }

        public bool IsOrdered { get; private set; }

        public void AddToCart(Product product, int amount)
        {
            if(amount < 0)
            {
                throw new InvalidOperationException();
            }

            var item = this.Items.SingleOrDefault(x => x.ProductId == product.Id);
            if(item == null)
            {
                this.Items.Add(new CartItem(product, amount));
            }
            else
            {
                item.ChangeAmount(product, amount);
            }

            this.Recalculate();
        }

        public void RemoveFromCart(Product product)
        {
            var item = this.Items.Single(x => x.ProductId == product.Id);
            this.Items.Remove(item);
            this.Recalculate();
        }

        public void RemoveFromCart(Product product, int amount)
        {
            var item = this.Items.Single(x => x.ProductId == product.Id);
            if(item.Amount <= amount)
            {
                RemoveFromCart(product);
            }
            else
            {
                item.ChangeAmount(product, -amount);
                this.Recalculate();
            }
        }

        public Order MakeOrder(IList<Product> products, Client client)
        {
            var order = new Order(this, client, OrderStatus.Submitted);
            this.IsOrdered = true;
            return order;
        }

        private void Recalculate()
        {
            this.Value = this.Items.Sum(x => x.Value);
        }
    }
}
