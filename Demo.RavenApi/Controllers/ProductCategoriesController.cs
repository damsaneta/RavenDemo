using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.Model.EF.Dtos;
using Demo.Model.Raven.Entities;
using Demo.RavenApi.Models;
using Demo.RavenApi.Models.DataTables;
using Raven.Client;

namespace Demo.RavenApi.Controllers
{
    public class ProductCategoriesController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        public IHttpActionResult Get(string id)
        {
            var result = this.session.Load<ProductCategory>(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [ResponseType(typeof(IList<ProductCategory>))]
        public IHttpActionResult Get(DtRequest<ProductCategory> request)
        {
            var result = this.session.Query<ProductCategory>().ToList();
            return this.Ok(result);
        }

        public IHttpActionResult Post([FromBody]ProductCategory productCategory)
        {
            this.session.Store(productCategory);
            this.session.SaveChanges();
            return this.Ok(productCategory.Id);
        }
        
        public IHttpActionResult Put([FromBody]ProductCategory productCategory)
        {
            ProductCategory currentCategory = this.session.Load<ProductCategory>(productCategory.Id);
            if (currentCategory == null)
            {
                return this.NotFound();
            }

            currentCategory.Name = productCategory.Name;
            this.session.SaveChanges();
            return this.Ok(currentCategory);
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