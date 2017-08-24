using Demo.Model.Raven.Entities;

namespace Demo.Model.Raven.Dtos
{

    public class ProductInventoryDto
    {
        public ProductInventoryDto()
        {
            
        }

        public ProductInventoryDto(ProductInventory entity, string productName, string locationName)
        {
            this.ProductId = entity.ProductId;
            this.LocationId = entity.LocationId;
            this.Shelf = entity.Shelf;
            this.Bin = entity.Bin;
            this.Quantity = entity.Quantity;
            this.ProductName = productName;
            this.LocationName = locationName;
        }

        public string ProductId { get; set; }

        public string LocationId { get; set; }

        public string Shelf { get; set; }

        public byte Bin { get; set; }

        public short Quantity { get; set; }

        public string ProductName { get; set; }

        public string LocationName { get; set; }
    }
}
