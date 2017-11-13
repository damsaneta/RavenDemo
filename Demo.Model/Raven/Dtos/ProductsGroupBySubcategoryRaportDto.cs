using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.Raven.Dtos
{
    public class ProductsGroupBySubcategoryRaportDto
    {
        public ProductsGroupBySubcategoryRaportDto()
        {
            
        }

        public string ProductSubcategoryId { get; set; }

        public string ProductSubcategoryName { get; set; }

        public int Count { get; set; }
    }
}
