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
    public class ProductsController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult Get(int id)
        {
            ProductDto produtcDto = db.Database.SqlQuery<ProductDto>(
                @"SELECT
                  p.[ID]
                , p.[Name]
                , p.[ProductNumber]
                , p.[Color]
                , p.[ListPrice]
                , p.[Size]
                , p.[SizeUnitMeasureCode]
                , p.[WeightUnitMeasureCode]
                , p.[Weight]
                , p.[ProductLine]
                , p.[Class]
                , p.[Style]
                , p.[ProductSubcategoryID]
                , ps.ID as ProductSubcategoryID
                , ps.Name as ProductSubcategoryName
                FROM Product as p LEFT JOIN ProductSubcategory as ps
                ON p.ProductSubcategoryID = ps.ID WHERE p.ID=@p0", id).SingleOrDefault();
            if(produtcDto == null)
            {
                return NotFound();
            }

            return Ok(produtcDto);
        }
        [ResponseType(typeof(IList<ProductDto>))]
        public IHttpActionResult Get(DtRequest<ProductDto> request)
        {
            var parameters = new List<object>();

            var sql = @"SELECT p.[ID]
                , p.[Name]
                , p.[ProductNumber]
                , p.[Color]
                , p.[ListPrice]
                , p.[Size]
                , p.[SizeUnitMeasureCode]
                , p.[WeightUnitMeasureCode]
                , p.[Weight]
                , p.[ProductLine]
                , p.[Class]
                , p.[Style]
                , p.[ProductSubcategoryID]
                , ps.ID as ProductSubcategoryID
                , ps.Name as ProductSubcategoryName
                FROM Product as p LEFT JOIN ProductSubcategory as ps
                ON p.ProductSubcategoryID = ps.ID";

            if (!string.IsNullOrEmpty(request.Search))
            {
                sql += " WHERE p.Name LIKE @p0 OR ps.Name LIKE @p0 ";
                parameters.Add(request.Search + "%");
            }

            request.OrderColumn = request.OrderColumn ?? "Name";
            sql += " ORDER BY " + request.OrderColumn + " " + request.OrderDirection;

            var result = db.Database.SqlQuery<ProductDto>(sql, parameters.ToArray()).ToList();

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

        //SELECT p.[ID]
        //    , p.[Name]
        //    , p.[ProductNumber]
        //    , p.[Color]
        //    , p.[ListPrice]
        //    , p.[Size]
        //    , p.[SizeUnitMeasureCode]
        //    , p.[WeightUnitMeasureCode]
        //    , p.[Weight]
        //    , p.[ProductLine]
        //    , p.[Class]
        //    , p.[Style]
        //    , p.[ProductSubcategoryID]
        //FROM [dbo].[Product] p
    }
}
