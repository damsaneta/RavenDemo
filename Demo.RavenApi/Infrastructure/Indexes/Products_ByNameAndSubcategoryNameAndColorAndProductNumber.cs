using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Products_ByNameAndSubcategoryNameAndColorAndProductNumber : AbstractIndexCreationTask<Product>
    {
        public class Result
        {
            public string Name { get; set; }

            public string ProductSubcategoryName { get; set; }

            public string ProductNumber { get; set; }

            public string Color { get; set; }
        }

        public Products_ByNameAndSubcategoryNameAndColorAndProductNumber()
        {
            Map = products => from product in products
                select new
                {
                    Name = product.Name,
                    ProductSubcategoryName = LoadDocument<ProductSubcategory>(product.ProductSubcategoryId).Name,
                    ProductNumber = product.ProductNumber,
                    Color = product.Color                   
                };
            this.Store("Name", FieldStorage.Yes);
            this.Store("ProductSubcategoryName", FieldStorage.Yes);
            this.Store("ProductNumber", FieldStorage.Yes);
            this.Store("Color", FieldStorage.Yes);
        }
    }
}