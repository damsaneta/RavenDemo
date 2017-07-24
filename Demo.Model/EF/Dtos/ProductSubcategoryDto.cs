namespace Demo.Model.EF.Dtos
{
    public class ProductSubcategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }
    }
}