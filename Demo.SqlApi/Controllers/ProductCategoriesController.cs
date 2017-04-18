using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.SqlApi.Model.Dtos;
using Demo.SqlApi.Model.Entities;

namespace Demo.SqlApi.Controllers
{
    public class ProductCategoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [ResponseType(typeof(IList<ProductCategoryDto>))]
        public IHttpActionResult Get(string name = null)
        {
            List<ProductCategoryDto> result;
            if (!string.IsNullOrEmpty(name))
            {
                result = db.Database.SqlQuery<ProductCategoryDto>("SELECT ID, Name FROM dbo.ProductCategory WHERE Name like @p0 ORDER BY Name ASC", name + "%").ToList();
            }
            else
            {
                result = db.Database.SqlQuery<ProductCategoryDto>("SELECT ID, Name FROM dbo.ProductCategory ORDER BY Name ASC").ToList();
            }
            
            return this.Ok(result);
        }

        [ResponseType(typeof(ProductCategoryDto))]
        public IHttpActionResult Get(int id)
        {
            ProductCategoryDto productCategory =
                db.Database.SqlQuery<ProductCategoryDto>("SELECT ID, Name FROM dbo.ProductCategory WHERE ID=@p0", id)
                    .SingleOrDefault();
            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(productCategory);
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