using Demo.Domain.Shared;

namespace Demo.Domain.Users
{
    public class User : Entity
    {
        protected User()
        {
        }

        public User(string userName, string firstName, string lastName, byte[] password, Role role)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Role = role;
        }

        public string UserName { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public byte[] Password { get; private set; }

        public Role Role { get; private set; }
    }
}
