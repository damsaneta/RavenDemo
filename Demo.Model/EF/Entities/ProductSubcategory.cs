using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Model.EF.Entities
{
    [Table("ProductSubcategory")]
    public class ProductSubcategory
    {
        //public ProductSubcategory()
        //{
        //    Product = new HashSet<Product>();
        //}
        public int ID { get; set; }

        public int ProductCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

       // public virtual ICollection<Product> Product { get; set; }
    }
}