using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class ProductCategories_ByNameSortByIdName : AbstractIndexCreationTask<ProductCategory>
    {
        public ProductCategories_ByNameSortByIdName()
        {
            Map = productCategories => from productCategory in productCategories
                select new
                {
                    __document_id = productCategory.Id
                };
        }
    }
}