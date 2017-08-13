using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Products_ByNameAndColorAndProductNumber : AbstractIndexCreationTask<Product>
    {
        public Products_ByNameAndColorAndProductNumber()
        {
            Map = products => from product in products
                select new
                {
                    Name = product.Name,
                    ProductNumber = product.ProductNumber,
                    Color = product.Color                   
                };
        }
    }
}