using System.Linq;
using Demo.Domain.Users;
using Demo.Storage.Infrastructure;

namespace Demo.Storage.Repositories.Impl
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDocumentSessionProvider provider)
            : base(provider)
        {
        }

        public User GetByUserName(string userName)
        {
            return this.DocumentSession.Query<User>().FirstOrDefault(x => x.UserName == userName);
        }
    }
}