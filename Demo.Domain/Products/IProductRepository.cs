using System.Collections.Generic;
using Demo.Common.Dtos.Products;
using Demo.Domain.Shared;

namespace Demo.Domain.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        IList<ProductGridItemDto> GetListForGrid();
    }
}
