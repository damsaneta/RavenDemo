using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class ProductCategories_ByName : AbstractIndexCreationTask<ProductCategory>
    {
        public ProductCategories_ByName()
        {
            Map = productCategories => from productCategory in productCategories
                select new
                {
                    Name = productCategory.Name
                };
        }
    }
}