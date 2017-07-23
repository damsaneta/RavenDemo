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

namespace Demo.RavenApi.Controllers
{
    public class ProductSubcategoriesController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(ProductSubcategoryDto))]
        public IHttpActionResult Get(string id)
        {
            var result = this.session.Load<ProductSubcategory>("ProductSubcategories/" + id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ProductSubcategoryDto(result));
        }

        [ResponseType(typeof(IList<ProductCategoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductCategoryDto> request)
        {
            throw new NotImplementedException();
        }

        public IHttpActionResult Post([FromBody]ProductSubcategoryDto productSubcategoryDto)
        {
            var entity = new ProductSubcategory(productSubcategoryDto);
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