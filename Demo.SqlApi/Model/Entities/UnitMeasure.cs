namespace Demo.SqlApi.Model.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 
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