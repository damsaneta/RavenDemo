using Demo.Model.Raven.Dtos;

namespace Demo.Model.Raven.Entities
{
    public class Product
    {
        public Product()
        {
            
        }

        public Product(ProductDto dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.ProductNumber = dto.ProductNumber;
            this.Color = dto.Color;
            this.ListPrice = dto.ListPrice;
            this.ProductSubcategoryId = dto.ProductSubcategoryId;
            this.ProductSubcategoryName = dto.ProductSubcategoryName;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

        public string Color { get; set; }

        public decimal ListPrice { get; set; }

        public string ProductSubcategoryId { get; set; }

        public string  ProductSubcategoryName { get; set; }
    }
}
