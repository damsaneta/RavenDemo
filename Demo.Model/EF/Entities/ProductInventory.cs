using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Model.EF.Entities
{
    [Table("ProductInventory")]
    public class ProductInventory
    {
        [Key]
        [Column("ProductID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [Key]
        [Column("LocationID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short LocationId { get; set; }

        [Required]
        [StringLength(10)]
        public string Shelf { get; set; }

        public byte Bin { get; set; }

        public short Quantity { get; set; }

        public virtual Location Location { get; set; }

        public virtual Product Product { get; set; }
    }
}
