using Demo.Domain.Orders;
using Demo.Storage.Repositories;

namespace Demo.ApplicationLogic.Impl
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }
    }
}
