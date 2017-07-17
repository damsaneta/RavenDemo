using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Model.EF.Entities
{
    [Table("UnitMeasure")]
    public class UnitMeasure
    {
        [Key]
        [StringLength(3)]
        public string UnitMeasureCode { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}