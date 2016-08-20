using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain.Shared;
using Demo.Domain.Users;

namespace Demo.Domain.Orders
{
    public class Order : Entity
    {
        public Order(Cart cart, Client client, OrderStatus status)
        {
            this.CartId = cart.Id;
            this.Client = client;
            this.Status = status;
        }

        public string CartId { get; private set; }

        public Client Client { get; private set; }

        public OrderStatus Status { get; private set; }

        public IList<OrderItem> OrderItems { get; private set; }
    }
}
