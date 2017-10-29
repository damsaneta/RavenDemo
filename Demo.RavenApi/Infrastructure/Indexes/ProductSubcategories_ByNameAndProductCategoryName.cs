using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class ProductSubcategories_ByNameAndProductCategoryName : AbstractIndexCreationTask<ProductSubcategory>
    {
        public class Result
        {
            public string Name { get; set; }

            public string ProductCategoryName { get; set; }
        }

        public ProductSubcategories_ByNameAndProductCategoryName()
        {
            Map = productSubcategories => from productSubcategory in productSubcategories
                select new Result
                {
                    Name = productSubcategory.Name,
                    ProductCategoryName = LoadDocument<ProductCategory>(productSubcategory.ProductCategoryId).Name
                };
            this.Store("Name", FieldStorage.Yes);
            this.Store("ProductCategoryName", FieldStorage.Yes);

        }
    }
}