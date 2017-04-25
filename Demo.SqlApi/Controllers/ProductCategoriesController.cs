using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.SqlApi.Model.DataTables;
using Demo.SqlApi.Model.Dtos;
using Demo.SqlApi.Model.Entities;

namespace Demo.SqlApi.Controllers
{
    public class ProductCategoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

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

        [ResponseType(typeof(IList<ProductCategoryDto>))]
        public IHttpActionResult Get(DtRequest request)
        {
            var parameters = new List<object>();
            var sql = "SELECT ID, Name FROM dbo.ProductCategory ";
            if (!string.IsNullOrEmpty(request.Search))
            {
                sql += "WHERE Name LIKE @p0 ";
                parameters.Add(request.Search);
            }

            request.OrderColumn = request.OrderColumn ?? "Name";
            sql += "ORDER BY " + request.OrderColumn + " " + request.OrderDirection;
            var result = db.Database.SqlQuery<ProductCategoryDto>(sql, parameters.ToArray()).ToList();

            return this.Ok(result);
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