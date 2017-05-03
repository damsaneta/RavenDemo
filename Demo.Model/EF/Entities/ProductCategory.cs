using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Model.Entities
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        //public ProductCategory()
        //{
        //    ProductSubcategory = new HashSet<ProductSubcategory>();
        //}

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        //public virtual ICollection<ProductSubcategory> ProductSubcategory { get; set; }
    }
}
