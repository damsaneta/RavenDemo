using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.SqlApi.Model.Dtos
{
    public class ProductSubcategoryDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int ProductCategoryID { get; set; }

        public string ProductCategoryName { get; set; }
    }
}