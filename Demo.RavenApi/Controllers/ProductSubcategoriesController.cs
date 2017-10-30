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
using Demo.RavenApi.Infrastructure.Indexes;
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

            var productCategoryName = this.session.Load<ProductCategory>(result.ProductCategoryId).Name;

            return this.Ok(new ProductSubcategoryDto(result, productCategoryName));
        }

        [ResponseType(typeof (IList<ProductSubcategoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductSubcategoryDto> request)
        {
            IRavenQueryable<ProductSubcategories_ByNameAndProductCategoryName.Result> indexQuery = this.session.Query<ProductSubcategories_ByNameAndProductCategoryName.Result,
                        ProductSubcategories_ByNameAndProductCategoryName>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(
                        x => x.Name.StartsWith(request.Search) || x.ProductCategoryName.StartsWith(request.Search));
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "ProductCategoryName", request.OrderDirection == DtOrderDirection.DESC));
            List<ProductSubcategoryDto> result = indexQuery.ProjectFromIndexFieldsInto<ProductSubcategoryDto>()
                .ToList();
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