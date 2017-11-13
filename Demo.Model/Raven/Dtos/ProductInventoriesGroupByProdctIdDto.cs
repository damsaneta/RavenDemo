using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.Raven.Dtos
{
    public class ProductInventoriesGroupByProdctIdDto
    {
        public ProductInventoriesGroupByProdctIdDto()
        {
            
        }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int LocationCount { get; set; }

        public int QuantityCount { get; set; }
    }
}
