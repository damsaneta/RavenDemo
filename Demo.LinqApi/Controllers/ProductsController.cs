using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.LinqApi.Model;
using Demo.LinqApi.Model.DataTables;
using Demo.Model.EF.Dtos;
using Demo.Model.EF.Entities;

namespace Demo.LinqApi.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult Get(int id)
        {
            var products = db.Set<Product>().Where(x => x.Id == id);
            var prodSubCat = db.Set<ProductSubcategory>();

            var result = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductNumber = product.ProductNumber,
                Color = product.Color,
                ListPrice = product.ListPrice,
                ProductSubcategoryId = product.ProductSubcategoryId,
                ProductSubcategoryName = product.ProductSubcategory.Name,
                SafetyStockLevel = product.SafetyStockLevel,
                ReorderPoint = product.ReorderPoint,
                SellStartDate = product.SellStartDate,
                SellEndDate = product.SellEndDate,
                Size = product.Size,
                SizeUnitMeasureCode = product.SizeUnitMeasureCode,
                Weight = product.Weight,
                WeightUnitMeasureCode = product.WeightUnitMeasureCode

            }).SingleOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [ResponseType(typeof(IList<ProductDto>))]
        public IHttpActionResult Get(DtRequest<ProductDto> request)
        {

            IQueryable<Product> products = db.Set<Product>();
            IQueryable<ProductSubcategory> prodSubCat = db.Set<ProductSubcategory>();

            IQueryable<ProductDto> queryDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductNumber = product.ProductNumber,
                Color = product.Color,
                ListPrice = product.ListPrice,
                ProductSubcategoryId = product.ProductSubcategoryId,
                ProductSubcategoryName = product.ProductSubcategory.Name,
                SafetyStockLevel = product.SafetyStockLevel,
                ReorderPoint = product.ReorderPoint,
                SellStartDate = product.SellStartDate,
                SellEndDate = product.SellEndDate,
                Size = product.Size,
                SizeUnitMeasureCode = product.SizeUnitMeasureCode,
                Weight = product.Weight,
                WeightUnitMeasureCode = product.WeightUnitMeasureCode
            });

            if (!string.IsNullOrEmpty(request.Search))
            {
                queryDto = queryDto.Where(x => x.Name.StartsWith(request.Search) || x.ProductSubcategoryName.StartsWith(request.Search)
                || x.ProductNumber.StartsWith(request.Search) || x.Color.StartsWith(request.Search));
            }

            switch (request.OrderColumn)
            {
                case "Id":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.Id);
                    break;
                case "ProductSubcategoryId":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.ProductSubcategoryId);
                    break;
                case "ProductSubcategoryName":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductSubcategoryName)
                        : queryDto.OrderByDescending(x => x.ProductSubcategoryName);
                    break;
                case "Color":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Color)
                        : queryDto.OrderByDescending(x => x.Color);
                    break;
                case "ProductNumber":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductNumber)
                        : queryDto.OrderByDescending(x => x.ProductNumber);
                    break;
                case "ListPrice":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ListPrice)
                        : queryDto.OrderByDescending(x => x.ListPrice);
                    break;
                default:
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Name)
                        : queryDto.OrderByDescending(x => x.Name);
                    break;
            }

            var result = queryDto.ToList();

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
