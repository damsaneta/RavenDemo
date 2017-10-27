using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class ProductSubcategories_ByNameAndProductCategoryName : AbstractIndexCreationTask<ProductSubcategory>
    {
        public ProductSubcategories_ByNameAndProductCategoryName()
        {
            Map = productSubcategories => from productSubcategory in productSubcategories
                select new
                {
                    Name = productSubcategory.Name,
                    ProductCategoryName = LoadDocument<ProductCategory>(productSubcategory.ProductCategoryId).Name
                };
        }
    }
}