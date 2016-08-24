using Demo.Domain.Shared;

namespace Demo.Domain.Orders
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetCartForUser(string userId);
    }
}
