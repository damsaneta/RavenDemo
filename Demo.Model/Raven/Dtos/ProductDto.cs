using System;
using Demo.Model.Raven.Entities;

namespace Demo.Model.Raven.Dtos
{
    public class ProductDto
    {
        public ProductDto()
        {
            
        }

        public ProductDto(Product entity, string productSubcategoryName)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.ProductNumber = entity.ProductNumber;
            this.Color = entity.Color;
            this.ListPrice = entity.ListPrice;
            this.ProductSubcategoryId = entity.ProductSubcategoryId;
            this.ProductSubcategoryName = productSubcategoryName;
            this.ReorderPoint = entity.ReorderPoint;
            this.SafetyStockLevel = entity.SafetyStockLevel;
            this.SellStartDate = entity.SellStartDate;
            this.SellEndDate = entity.SellEndDate;
            this.Size = entity.Size;
            this.SizeUnitMeasureCode = entity.SizeUnitMeasureCode;
            this.WeightUnitMeasureCode = WeightUnitMeasureCode;
            this.Weight = entity.Weight;

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

        public string ProductSubcategoryName { get; set; }

    }
}
