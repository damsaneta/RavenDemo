using Demo.Model.Raven.Entities;

namespace Demo.Model.Raven.Dtos
{
    public class ProductCategoryDto
    {
        public ProductCategoryDto()
        {
        }

        public ProductCategoryDto(ProductCategory entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}