using System;
using System.Collections.Generic;
using Demo.Domain.Shared;
using Demo.Domain.Users;

namespace Demo.Domain.Orders
{
    public class Order : Entity
    {
        public Order(string cartId, IList<OrderItem> orderItems, Client client)
        {
            this.CartId = cartId;
            this.OrderItems = orderItems;
            this.Client = client;
            this.Status = OrderStatus.New;
            this.OrderDate = DateTime.Now;
        }

        public string CartId { get; private set; }

        public Client Client { get; private set; }

        public OrderStatus Status { get; private set; }

        public DateTime OrderDate { get; private set; }

        public IList<OrderItem> OrderItems { get; private set; }
    }
}
