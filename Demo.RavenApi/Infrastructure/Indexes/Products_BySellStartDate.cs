using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Products_BySellStartDate : AbstractIndexCreationTask<Product>
    {
        public class Result
        {
            public int YearOfSell { get; set; }
        }

        public Products_BySellStartDate()
        {
            Map = products => from product in products
                select new
                {
                    YearOfSell = product.SellStartDate.Year
                };
        }
    }
}