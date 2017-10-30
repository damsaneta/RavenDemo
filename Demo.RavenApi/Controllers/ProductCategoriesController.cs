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
    public class ProductCategoriesController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(ProductCategoryDto))]
        public IHttpActionResult Get(string id)
        {
            var result = this.session.Load<ProductCategory>(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ProductCategoryDto(result));
        }

        [ResponseType(typeof(IList<ProductCategoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductCategoryDto> request)
        {
            IRavenQueryable<ProductCategory> indexQuery = this.session.Query<ProductCategory,
           ProductCategories_ByName>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(x => x.Name.StartsWith(request.Search));
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "Name", request.OrderDirection == DtOrderDirection.DESC));
            List<ProductCategoryDto> result = indexQuery.ProjectFromIndexFieldsInto<ProductCategoryDto>()
                .ToList();
           
            return this.Ok(result);
        }

        public IHttpActionResult Post([FromBody]ProductCategoryDto productCategoryDto)
        {
            var entity = new ProductCategory(productCategoryDto);
            this.session.Store(entity);
            this.session.SaveChanges();
            return this.Ok(entity.Id);
        }

        public IHttpActionResult Put([FromBody]ProductCategoryDto productCategoryDto)
        {
            ProductCategory entity = this.session.Load<ProductCategory>(productCategoryDto.Id);
            if (entity == null)
            {
                return this.NotFound();
            }

            entity.Name = productCategoryDto.Name;
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