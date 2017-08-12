using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class ProductCategories_ByIdSortById : AbstractIndexCreationTask<ProductCategory>
    {
        public ProductCategories_ByIdSortById()
        {
            Map = productCategories => from productCategory in productCategories
                select new
                {
                    Name = productCategory.Name
                };
        }
    }
}