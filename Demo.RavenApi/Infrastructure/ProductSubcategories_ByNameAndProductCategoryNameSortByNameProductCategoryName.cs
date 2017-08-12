using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class ProductSubcategories_ByNameAndProductCategoryNameSortByNameProductCategoryName :
        AbstractIndexCreationTask<ProductSubcategory>
    {
        public ProductSubcategories_ByNameAndProductCategoryNameSortByNameProductCategoryName()
        {
            Map = productSubcategories => from productSubcategory in productSubcategories
                select new
                {
                    //ProductCategoryName = productSubcategory.ProductCategoryName,
                    Name = productSubcategory.Name
                };
        }
    }
}