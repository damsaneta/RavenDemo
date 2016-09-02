using System.Collections.Generic;
using System.Linq;
using Demo.Common.Dtos.Products;
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

        public IList<ProductGridItemDto> GetListForGrid()
        {
            return this.DocumentSession.Query<Product>()
                .Select(x => new ProductGridItemDto {Id = x.Id, Name = x.Name, Price = x.Price})
                .ToList();
        }
    }
}
