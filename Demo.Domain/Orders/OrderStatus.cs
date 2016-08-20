using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Orders
{
    public enum OrderStatus
    {
        Unknown = 0,
        Submitted = 1,
        Shipped = 2,
        Received = 3
    }
}
