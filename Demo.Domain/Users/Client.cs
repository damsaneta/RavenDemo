using Demo.Domain.Shared;

namespace Demo.Domain.Users
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
