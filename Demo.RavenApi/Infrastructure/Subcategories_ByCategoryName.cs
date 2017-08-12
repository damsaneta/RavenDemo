using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Subcategories_ByCategoryName : AbstractIndexCreationTask<ProductSubcategory>
    {
        public class Result
        {
            public string ProductCategoryName { get; set; }
        }

        public Subcategories_ByCategoryName()
        {
            Map = productSubcategories => from subcategory in productSubcategories
                                          select new 
                                          {
                                              ProductCategoryName = LoadDocument<ProductCategory>(subcategory.ProductCategoryId).Name
                                          };
        }
    }
}