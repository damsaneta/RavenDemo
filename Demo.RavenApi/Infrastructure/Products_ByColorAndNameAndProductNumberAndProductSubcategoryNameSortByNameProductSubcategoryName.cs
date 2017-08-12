using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Products_ByColorAndNameAndProductNumberAndProductSubcategoryNameSortByNameProductSubcategoryName :
        AbstractIndexCreationTask<Product>
    {
        public Products_ByColorAndNameAndProductNumberAndProductSubcategoryNameSortByNameProductSubcategoryName()
        {
            Map = products => from product in products
                select new
                {
                    //ProductSubcategoryName = product.ProductSubcategoryName,
                    ProductNumber = product.ProductNumber,
                    Color = product.Color,
                    Name = product.Name
                };
        }
    }
}