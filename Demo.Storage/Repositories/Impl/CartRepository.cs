using System;
using System.Linq;
using Demo.Domain.Orders;
using Demo.Domain.Users;
using Raven.Client.Document;

namespace Demo.Storage.Repositories.Impl
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(DocumentStore documentStore)
            : base(documentStore)
        {
        }

        public Cart GetCartForUser(string userId)
        {
            using (var session = DocumentStore.OpenSession())
            {
                var client = session.Query<Client>().FirstOrDefault(x => x.UserId == userId);
                return session.Query<Cart>().FirstOrDefault(x => x.ClientId == client.Id);
            }
        }
    }
}