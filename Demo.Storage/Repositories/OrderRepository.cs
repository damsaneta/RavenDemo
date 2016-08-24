using Demo.Domain.Orders;
using Demo.Storage.Infrastructure;

namespace Demo.Storage.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IDocumentSessionProvider provider) 
            : base(provider)
        {
        }
    }
}