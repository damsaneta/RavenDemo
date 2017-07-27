using Demo.Model.Raven.Entities;

namespace Demo.Model.Raven.Dtos
{
    public class ProductDto
    {
        public ProductDto()
        {
            
        }

        public ProductDto(Product entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.ProductNumber = entity.ProductNumber;
            this.Color = entity.Color;
            this.ListPrice = entity.ListPrice;
            this.ProductSubcategoryId = entity.ProductSubcategoryId;
            this.ProductSubcategoryName = entity.ProductSubcategoryName;

        }
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

        public string Color { get; set; }

        public decimal ListPrice { get; set; }

        public string ProductSubcategoryId { get; set; }

        public string ProductSubcategoryName { get; set; }

    }
}
