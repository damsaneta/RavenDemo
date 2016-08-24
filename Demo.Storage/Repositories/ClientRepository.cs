using Demo.Domain.Users;
using Demo.Storage.Infrastructure;

namespace Demo.Storage.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(IDocumentSessionProvider provider) 
            : base(provider)
        {
        }
    }
}
