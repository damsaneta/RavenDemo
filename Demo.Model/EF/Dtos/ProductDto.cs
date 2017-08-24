using System;

namespace Demo.Model.EF.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

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

        public int? ProductSubcategoryId { get; set; }

        public string ProductSubcategoryName { get; set; }

    }
}
