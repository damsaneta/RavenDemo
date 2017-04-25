using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.SqlApi.Model.Dtos;
using Demo.SqlApi.Model.Entities;

namespace Demo.SqlApi.Controllers
{
    public class ProductSubcategoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [ResponseType(typeof(ProductSubcategoryDto))]
        public IHttpActionResult Get(int id)
        {
            ProductSubcategoryDto productSubcategory =
                db.Database.SqlQuery<ProductSubcategoryDto>(
                    @"SELECT ps.ID
                    , ps.ProductCategoryID
                    , ps.Name
                    , pc.Name AS ProductCategoryName
                    FROM dbo.ProductSubcategory ps
                    INNER JOIN dbo.ProductCategory pc ON ps.ProductCategoryID = pc.ID WHERE ps.ID=@p0", id)
                    .SingleOrDefault();
            if (productSubcategory == null)
            {
                return NotFound();
            }

            return Ok(productSubcategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
