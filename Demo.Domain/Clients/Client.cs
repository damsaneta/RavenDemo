using Demo.Domain.Shared;
using Demo.Domain.Users;

namespace Demo.Domain.Clients
{
    public class Client : Entity
    {
        protected Client()
        {
        }

        public Client(User user, Address address)
        {
            UserId = user.Id;
            Address = address;
        }

        public string UserId { get; private set; }

        public Address Address { get; private set; }
    }
}
