using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class Products_ByNameAndColor : AbstractIndexCreationTask<Product>
    {
        public Products_ByNameAndColor()
        {
            Map = products => from product in products
                select new
                {
                    Name = product.Name.Boost(15),
                    Color = product.Color.Boost(5)
                };

            Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
            Indexes.Add(x => x.Color, FieldIndexing.NotAnalyzed);

        }
    }
}