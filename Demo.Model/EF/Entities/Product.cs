using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Model.EF.Entities
{
    [Table("Product")]
    public class Product
    {
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(25)]
        public string ProductNumber { get; set; }

        [StringLength(15)]
        public string Color { get; set; }

        [Column(TypeName = "money")]
        public decimal ListPrice { get; set; }

        [Column("ProductSubcategoryID")]
        public int? ProductSubcategoryId { get; set; }

        //public DateTime SellStartDate { get; set; }

        //public DateTime? SellEndDate { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ProductSubcategory ProductSubcategory { get; set; }
    }
}
