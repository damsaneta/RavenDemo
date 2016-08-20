using Demo.Domain.Orders;

namespace Demo.Storage.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetCartForUser(string userId);
    }
}
