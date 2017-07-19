using Demo.Model.Raven.Dtos;

namespace Demo.Model.Raven.Entities
{
    public class ProductCategory
    {
        public ProductCategory()
        {
        }

        public ProductCategory(ProductCategoryDto dto)
        {
            this.Id = "productCategories/" + dto.Id;
            this.Name = dto.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
