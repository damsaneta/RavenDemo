using System;
using System.Collections.Generic;

namespace Demo.SqlApi.Model.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
