using Demo.Model.Raven.Entities;

namespace Demo.Model.Raven.Dtos
{
    public class ProductSubcategoryDto
    {
        public ProductSubcategoryDto()
        {
        }

        public ProductSubcategoryDto(ProductSubcategory entity, string categoryName)
        {
            this.Id = entity.Id;
            this.ProductCategoryId = entity.ProductCategoryId;
            this.Name = entity.Name;
            this.ProductCategoryName = categoryName;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }
    }
}
