using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class ProductSubcategories_ByProductCategoryName : AbstractIndexCreationTask<ProductSubcategory>
    {
        public class Result
        {
            public string ProductCategoryName { get; set; }
        }

        public ProductSubcategories_ByProductCategoryName()
        {
            Map = productSubcategories => from subcategory in productSubcategories
                                          select new 
                                          {
                                              ProductCategoryName = LoadDocument<ProductCategory>(subcategory.ProductCategoryId).Name
                                          };
            this.Index("ProductCategoryName", FieldIndexing.Analyzed);
        }
    }
}