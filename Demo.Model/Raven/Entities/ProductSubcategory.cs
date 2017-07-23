using Demo.Model.Raven.Dtos;

namespace Demo.Model.Raven.Entities
{
    public class ProductSubcategory
    {
        public ProductSubcategory()
        {    
        }

        public ProductSubcategory(ProductSubcategoryDto dto)
        {
            this.Id = "ProductSubcategories/" + dto.Id;
            this.ProductCategoryId = "ProductCategories/" + dto.ProductCategoryId;
            this.Name = dto.Name;
        }

        public string Id { get; set; }

        public string ProductCategoryId { get; set; }

        public string Name { get; set; }
    }
}
