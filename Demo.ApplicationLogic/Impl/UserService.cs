using Demo.Domain.Users;
using Demo.Storage.Repositories;

namespace Demo.ApplicationLogic.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
    }
}