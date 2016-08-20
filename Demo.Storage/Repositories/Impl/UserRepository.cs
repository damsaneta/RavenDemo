using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Demo.Domain.Users;
using Raven.Client.Document;

namespace Demo.Storage.Repositories.Impl
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DocumentStore documentStore)
            : base(documentStore)
        {
        }

        public User GetByUserName(string userName)
        {
            using (var session = DocumentStore.OpenSession())
            {
                return session.Query<User>().FirstOrDefault(x => x.UserName == userName);
            }
        }
    }
}