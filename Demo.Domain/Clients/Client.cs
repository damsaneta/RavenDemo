using Demo.Domain.Shared;
using Demo.Domain.Users;

namespace Demo.Domain.Clients
{
    public class Client : Entity
    {
        public Client(string name, User user, Address address)
        {
            Name = name;
            User = user;
            Address = address;
        }

        public string Name { get; private set; }

        public User User { get; private set; }

        public Address Address { get; private set; }
    }
}
