using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.EF.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

        public string Color { get; set; }

        public decimal ListPrice { get; set; }

        public int? ProductSubcategoryId { get; set; }

        public string ProductSubcategoryName { get; set; }

    }
}
