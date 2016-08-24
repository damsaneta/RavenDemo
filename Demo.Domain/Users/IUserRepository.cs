using Demo.Domain.Shared;

namespace Demo.Domain.Users
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUserName(string userName);
    }
}
