using System.Linq;
using Demo.Domain.Orders;
using Demo.Domain.Users;
using Demo.Storage.Infrastructure;

namespace Demo.Storage.Repositories.Impl
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(IDocumentSessionProvider provider)
            : base(provider)
        {
        }

        public Cart GetCartForUser(string userId)
        {
            var client = this.DocumentSession.Query<Client>().FirstOrDefault(x => x.UserId == userId);
            return this.DocumentSession.Query<Cart>().FirstOrDefault(x => x.ClientId == client.Id);
        }
    }
}