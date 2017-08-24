using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class ProductSubcategories_ByNameAndProductCategoryId : AbstractIndexCreationTask<ProductSubcategory>
    {
        public ProductSubcategories_ByNameAndProductCategoryId()
        {
            Map = productSubcategories => from productSubcategory in productSubcategories
                select new
                {
                    Name = productSubcategory.Name,
                    ProductCategoryId = productSubcategory.ProductCategoryId
                };
        }
    }
}