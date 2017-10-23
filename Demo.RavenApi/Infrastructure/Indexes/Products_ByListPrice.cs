using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class Products_ByListPrice : AbstractIndexCreationTask<Product>
    {
        public Products_ByListPrice()
        {
            Map = products => from product in products
                select new
                {
                    ListPrice = product.ListPrice
                };

            Sort(x => x.ListPrice, SortOptions.Double);
        }
    }
}