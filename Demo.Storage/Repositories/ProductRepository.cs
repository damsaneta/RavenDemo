using Demo.Domain.Products;
using Demo.Storage.Infrastructure;

namespace Demo.Storage.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IDocumentSessionProvider provider) 
            : base(provider)
        {
        }
    }
}
