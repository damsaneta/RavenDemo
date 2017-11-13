using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class ProductInventories_GroupByProdctId : AbstractIndexCreationTask<ProductInventory, ProductInventories_GroupByProdctId.Result>
    {

        public ProductInventories_GroupByProdctId()
        {
            Map = productInventories => from productInventory in productInventories
                select new Result
                {
                    ProductId = productInventory.ProductId,
                    ProductName = LoadDocument<Product>(productInventory.ProductId).Name ?? "Brak",
                    LocationCount = 1,
                    QuantityCount = productInventory.Quantity
                };

            Reduce = results => from result in results
                group result by new {result.ProductId, result.ProductName}
                into g
                select new Result
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    LocationCount = g.Sum(x => x.LocationCount),
                    QuantityCount = (short) g.Sum(y => y.QuantityCount)

                };

            this.Store("ProductId", FieldStorage.Yes);
            this.Store("ProductName", FieldStorage.Yes);
            this.Store("LocationCount", FieldStorage.Yes);
            this.Store("QuantityCount", FieldStorage.Yes);
        }

        public class Result
        {
            public string ProductId { get; set; }

            public string ProductName { get; set; }

            public int LocationCount { get; set; }

            public short QuantityCount { get; set; }
        }
    }
}