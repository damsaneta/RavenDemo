using Demo.Model.Raven.Dtos;

namespace Demo.Model.Raven.Entities
{

    public class ProductInventory
    {
        public ProductInventory()
        {
            
        }

        public ProductInventory(ProductInventoryDto dto)
        {
            this.ProductId = dto.ProductId;
            this.LocationId = dto.LocationId;
            this.Shelf = dto.Shelf;
            this.Bin = dto.Bin;
            this.Quantity = dto.Quantity;

        }

        public string ProductId { get; set; }

        public string LocationId { get; set; }

        public string Shelf { get; set; }

        public byte Bin { get; set; }

        public short Quantity { get; set; }
    }
}
