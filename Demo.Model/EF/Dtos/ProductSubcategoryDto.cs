namespace Demo.Model.EF.Dtos
{
    public class ProductSubcategoryDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int ProductCategoryID { get; set; }

        public string ProductCategoryName { get; set; }
    }
}