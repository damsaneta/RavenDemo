using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class ProductInventories_ByProductNameAndLocationName : AbstractIndexCreationTask<ProductInventory>
    {
        public class Result
        {
            public string LocationName { get; set; }

            public string ProductName { get; set; }

        }

        public ProductInventories_ByProductNameAndLocationName()
        {
            Map = productInventories => from productInventory in productInventories
                select new
                {
                    LocationName = LoadDocument<Location>(productInventory.LocationId).Name,
                    ProductName = LoadDocument<Product>(productInventory.ProductId).Name
                };
            this.Store("LocationName", FieldStorage.Yes);
            this.Store("ProductName", FieldStorage.Yes);
        }
    }
}