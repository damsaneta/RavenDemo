using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.Model.Raven.Entities;
using Demo.RavenApi.Models;
using Demo.RavenApi.Models.DataTables;
using Raven.Client;
using Demo.Model.Raven.Dtos;
using Demo.RavenApi.Infrastructure;
using Raven.Client.Linq;

namespace Demo.RavenApi.Controllers
{
    public class ProductSubcategoriesController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(ProductSubcategoryDto))]
        public IHttpActionResult Get(string id)
        {
            var result = this.session.Load<ProductSubcategory>(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ProductSubcategoryDto(result, "TODO"));
        }

        [ResponseType(typeof (IList<ProductSubcategoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductSubcategoryDto> request)
        {
            var result = new List<ProductSubcategoryDto>();
            IQueryable<ProductSubcategory> subcategoriesQuery;
            if (!string.IsNullOrEmpty(request.Search))
            {
                subcategoriesQuery = this.session.Query<ProductSubcategory, ProductSubcategories_ByName>()
                    .Include(x => x.ProductCategoryId)
                    .Search(x => x.Name, request.Search);
            }
            else
            {
                subcategoriesQuery = this.session.Query<ProductSubcategory, ProductSubcategories_ByName>()
                .Include(x => x.ProductCategoryId);
            }



            
            IQueryable<ProductSubcategoryDto> queryDto = subcategoriesQuery.Select(subcategory => new ProductSubcategoryDto
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                ProductCategoryId = subcategory.ProductCategoryId
            });
            
            switch (request.OrderColumn)
            {
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

            result = queryDto.ToList();
            foreach (ProductSubcategoryDto dto in result)
            {
                var category = this.session.Load<ProductCategory>(dto.ProductCategoryId);
                dto.ProductCategoryName = category.Name;
            }

            return this.Ok(result);
        }

        public IHttpActionResult Post([FromBody]ProductSubcategoryDto productSubcategoryDto)
        {
            var entity = new ProductSubcategory(productSubcategoryDto);
            this.session.Store(entity);
            this.session.SaveChanges();
            return this.Ok(entity.Id);
        }

        public IHttpActionResult Put([FromBody]ProductSubcategoryDto productSubcategoryDto)
        {
            ProductSubcategory entity = this.session.Load<ProductSubcategory>(productSubcategoryDto.Id);
            if (entity == null)
            {
                return this.NotFound();
            }

            entity.Name = productSubcategoryDto.Name;
            entity.ProductCategoryId = productSubcategoryDto.ProductCategoryId;
            this.session.SaveChanges();
            return this.Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                session.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}