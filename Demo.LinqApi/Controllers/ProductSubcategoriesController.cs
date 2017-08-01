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
            var productCat = db.Set<ProductCategory>();
            var prodSubCat = db.Set<ProductSubcategory>().Where(x => x.Id == id);

            var productSubcategory = prodSubCat.Select(subcategory => new ProductSubcategoryDto
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                ProductCategoryName = subcategory.ProductCategory.Name,
                ProductCategoryId = subcategory.ProductCategoryId

            }).SingleOrDefault();

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

            IQueryable<ProductSubcategoryDto> queryDto = prodSubCat.Select(subcategory => new ProductSubcategoryDto
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                ProductCategoryName = subcategory.ProductCategory.Name,
                ProductCategoryId = subcategory.ProductCategoryId
            });

            if (!string.IsNullOrEmpty(request.Search))
            {
                queryDto = queryDto.Where(x => x.Name.StartsWith(request.Search) || x.ProductCategoryName.StartsWith(request.Search));
            }

            switch (request.OrderColumn)
            {
                case "Id":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.Id);
                    break;
                case "ProductCategoryId":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.ProductCategoryId);
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
