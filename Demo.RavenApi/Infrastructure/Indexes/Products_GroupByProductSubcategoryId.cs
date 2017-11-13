using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class Products_GroupByProductSubcategoryId : AbstractIndexCreationTask<Product, Products_GroupByProductSubcategoryId.Result>
    {
        public Products_GroupByProductSubcategoryId()
        {
            Map = products => from product in products
                select new Result
                {
                    ProductSubcategoryId = product.ProductSubcategoryId,
                    ProductSubcategoryName = LoadDocument<ProductSubcategory>(product.ProductSubcategoryId).Name ?? "Brak",
                    Count = 1
                };
            Reduce = results => from result in results
                group result by new { result.ProductSubcategoryId, result.ProductSubcategoryName }
                into g
                select new Result
                {
                    ProductSubcategoryId = g.Key.ProductSubcategoryId,
                    Count = g.Sum(x => x.Count),
                    ProductSubcategoryName = g.Key.ProductSubcategoryName
                };

            this.Store("ProductSubcategoryId", FieldStorage.Yes);
            this.Store("ProductSubcategoryName", FieldStorage.Yes);
            this.Store("Count", FieldStorage.Yes);
        }

        public class Result
        {
            public string ProductSubcategoryId { get; set; }

            public string ProductSubcategoryName { get; set; }

            public int Count { get; set; }
        }
    }
}