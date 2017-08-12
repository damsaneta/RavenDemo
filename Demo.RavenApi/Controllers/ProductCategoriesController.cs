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
            IQueryable<ProductCategory> query = this.session.Query<ProductCategory>();
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.StartsWith(request.Search));
            }

            var queryDto = query.Select(x => new ProductCategoryDto
            {
                Id = x.Id,
                Name = x.Name
            });
            switch (request.OrderColumn)
            {
                case "Id":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.Id);
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