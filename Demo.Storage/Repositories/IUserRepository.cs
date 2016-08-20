using Demo.Domain.Users;

namespace Demo.Storage.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUserName(string userName);
    }
}
