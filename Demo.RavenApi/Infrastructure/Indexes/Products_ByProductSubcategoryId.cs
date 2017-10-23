using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Products_ByProductSubcategoryId : AbstractIndexCreationTask<Product>
    {
        public Products_ByProductSubcategoryId()
        {
            Map = products => from product in products
               select new
                {
                    ProductSubcategoryId = product.ProductSubcategoryId
                };

           // Sort(x => x.ProductSubcategoryId, SortOptions.Int);
        }
    }
}