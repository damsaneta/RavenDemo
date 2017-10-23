using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class ProductInventories_ByShelfAndBin : AbstractIndexCreationTask<ProductInventory>
    {
        public class Result
        {
            public string Place { get; set; }
        }

        public ProductInventories_ByShelfAndBin()
        {
            Map = productInventories => from productInventory in productInventories
            select new
            {
                Place = productInventory.Shelf + " " + productInventory.Bin
            };
        }
    }
}