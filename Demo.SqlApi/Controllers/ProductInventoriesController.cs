using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.Model.EF.Dtos;
using Demo.SqlApi.Model;
using Demo.SqlApi.Model.DataTables;

namespace Demo.SqlApi.Controllers
{
    public class ProductInventoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [Route("api/productInventories/{productId}/{locationId}")]
        [ResponseType(typeof(ProductInventoryDto))]
        public IHttpActionResult Get(int productId, int locationId)
        {
            ProductInventoryDto productInventoryDto = db.Database.SqlQuery<ProductInventoryDto>(
                @"SELECT pI.[ProductID]
                      ,pI.[LocationID]
                      ,pI.[Shelf]
                      ,pI.[Bin]
                      ,pI.[Quantity]
                      ,p.Name as ProductName
                      ,l.Name as LocationName
                  FROM ProductInventory as pI
                  LEFT JOIN Product as p ON p.ID = pI.ProductID 
                  LEFT JOIN Location as l ON l.ID = pI.LocationID
                  WHERE pI.ProductID = @p0 AND pI.LocationID = @p1
                ", productId, locationId).SingleOrDefault();

            if (productInventoryDto == null)
            {
                return NotFound();
            }

            return Ok(productInventoryDto);

        }

        [ResponseType(typeof(IList<ProductInventoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductInventoryDto> request)
        {
            var parameters = new List<object>();

            var sql = @"SELECT pI.[ProductID]
                      ,pI.[LocationID]
                      ,pI.[Shelf]
                      ,pI.[Bin]
                      ,pI.[Quantity]
                      ,p.Name as ProductName
                      ,l.Name as LocationName
                  FROM ProductInventory as pI
                  LEFT JOIN Product as p ON p.ID = pI.ProductID 
                  LEFT JOIN Location as l ON l.ID = pI.LocationID";

            if (!string.IsNullOrEmpty(request.Search))
            {
                sql += " WHERE p.Name LIKE @p0 OR l.Name LIKE @p0 ";
                parameters.Add(request.Search);
            }

            request.OrderColumn = request.OrderColumn ?? "ProductName";
            sql += " ORDER BY " + request.OrderColumn + " " + request.OrderDirection;

            var result = db.Database.SqlQuery<ProductInventoryDto>(sql, parameters.ToArray()).ToList();

            return Ok(result);
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
