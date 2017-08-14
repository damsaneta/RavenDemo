using System;
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
            this.ReorderPoint = dto.ReorderPoint;
            this.SafetyStockLevel = dto.SafetyStockLevel;
            this.SellStartDate = dto.SellStartDate;
            this.SellEndDate = dto.SellEndDate;
            this.Size = dto.Size;
            this.SizeUnitMeasureCode = dto.SizeUnitMeasureCode;
            this.WeightUnitMeasureCode = WeightUnitMeasureCode;
            this.Weight = dto.Weight;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

        public string Color { get; set; }

        public short SafetyStockLevel { get; set; }
 
        public short ReorderPoint { get; set; }

        public decimal ListPrice { get; set; }

        public string Size { get; set; }

        public string SizeUnitMeasureCode { get; set; }

        public string WeightUnitMeasureCode { get; set; }

        public decimal? Weight { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime? SellEndDate { get; set; }

        public string ProductSubcategoryId { get; set; }
    }
}
