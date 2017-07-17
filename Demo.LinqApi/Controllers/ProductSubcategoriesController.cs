using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.LinqApi.Model;
using Demo.LinqApi.Model.DataTables;
using Demo.Model.EF.Dtos;
using Demo.Model.EF.Entities;

namespace Demo.LinqApi.Controllers
{
    public class ProductSubcategoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [ResponseType(typeof(ProductSubcategoryDto))]
        public IHttpActionResult Get(int id)
        {
            var productCat = db.Set<ProductCategory>().ToList();
            var prodSubCat = db.Set<ProductSubcategory>().Where(x => x.ID == id).ToList();
            var productSubcategory = productCat.Join(prodSubCat,
                    category => category.ID,
                    subcategory => subcategory.ProductCategoryID,
                    (category, subcategory) => new ProductSubcategoryDto
                    {
                        ID = subcategory.ID,
                        Name = subcategory.Name,
                        ProductCategoryID = category.ID,
                        ProductCategoryName = category.Name

                    }
                )
                .SingleOrDefault();

            if (productSubcategory == null)
            {
                return NotFound();
            }

            return Ok(productSubcategory);
        }

        [ResponseType(typeof(IList<ProductSubcategoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductSubcategoryDto> request)
        {
     
            IQueryable<ProductCategory> productCat = db.Set<ProductCategory>();
            IQueryable<ProductSubcategory> prodSubCat = db.Set<ProductSubcategory>();
         
            IQueryable<ProductSubcategoryDto> queryDto = productCat.Join(prodSubCat,
                category => category.ID,
                subcategory => subcategory.ProductCategoryID,
                (category, subcategory) => new ProductSubcategoryDto
                {
                    ID = subcategory.ID,
                    Name = subcategory.Name,
                    ProductCategoryID = category.ID,
                    ProductCategoryName = category.Name

                }
            );

            if (!string.IsNullOrEmpty(request.Search))
            {
                queryDto = queryDto.Where(x => x.Name.StartsWith(request.Search) || x.ProductCategoryName.StartsWith(request.Search));
            }

            switch (request.OrderColumn)
            {
                case "ID":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ID)
                        : queryDto.OrderByDescending(x => x.ID);
                    break;
                case "ProductCategoryID":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ID)
                        : queryDto.OrderByDescending(x => x.ProductCategoryID);
                    break;
                case "Name":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Name)
                        : queryDto.OrderByDescending(x => x.Name);
                    break;
                default:
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductCategoryName)
                        : queryDto.OrderByDescending(x => x.ProductCategoryName);
                    break;
            }

            var result = queryDto.ToList();
            //var parameters = new List<object>();
            //var sql = @"SELECT ps.ID
            //        , ps.ProductCategoryID
            //        , ps.Name
            //        , pc.Name AS ProductCategoryName
            //        , pc.ID AS ProductCategoryID
            //        FROM dbo.ProductSubcategory ps
            //        INNER JOIN dbo.ProductCategory pc ON ps.ProductCategoryID = pc.ID ";
            //if (!string.IsNullOrEmpty(request.Search))
            //{
            //    sql += "WHERE ps.Name LIKE @p0  OR pc.Name LIKE @p0 ";
            //    parameters.Add(request.Search);
            //}

            //request.OrderColumn = request.OrderColumn ?? "ProductCategoryName";
            //sql += "ORDER BY " + request.OrderColumn + " " + request.OrderDirection;
            //var result = db.Database.SqlQuery<ProductSubcategoryDto>(sql, parameters.ToArray()).ToList();

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
