namespace Demo.SqlApi.Model.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
