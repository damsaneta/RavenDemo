using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Products_BySubcategoryName : AbstractIndexCreationTask<Product>
    {
        public class Result
        {
            public string ProductSubcategoryName { get; set; }
        }

        public Products_BySubcategoryName()
        {
            Map = products => from product in products
                select new
                {
                    ProductSubcategoryName = LoadDocument<ProductSubcategory>(product.ProductSubcategoryId).Name
                };
        }
    }
}