using Demo.Domain;
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

        public bool CanSignIn(string userName, string password)
        {
            var user = this.userRepository.GetByUserName(userName);
            return CryptoHelper.ComparePasswords(password ,user.Password);
        }
    }
}