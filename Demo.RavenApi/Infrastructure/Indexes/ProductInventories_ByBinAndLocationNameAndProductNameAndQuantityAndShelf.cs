using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Model.Raven.Entities;

namespace Demo.RavenApi.Infrastructure.Indexes
{
    public class ProductInventories_ByBinAndLocationNameAndProductNameAndQuantityAndShelf : AbstractIndexCreationTask<ProductInventory>
    {
        public ProductInventories_ByBinAndLocationNameAndProductNameAndQuantityAndShelf()
        {
            Map = productInventories => from productInventory in productInventories
                select new
                {
                    LocationName = LoadDocument<Location>(productInventory.LocationId).Name,
                    ProductName = LoadDocument<Product>(productInventory.ProductId).Name,
                    Quantity = productInventory.Quantity,
                    Bin = productInventory.Bin,
                    Shelf = productInventory.Shelf
                };
        }
    }
}