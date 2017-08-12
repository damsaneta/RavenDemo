using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Demo.Model.Raven.Dtos;
using Demo.Model.Raven.Entities;
using Demo.RavenApi.Infrastructure;
using Demo.RavenApi.Models;
using Raven.Client;

namespace Demo.RavenApi.Controllers
{
    public class ExamplesController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesAll()
        {
            var result = this.session.Query<ProductCategory>().ToList();
            return this.Ok(result);
        }

        [Route("api/examples/categories/{id}")]
        public IHttpActionResult GetCategoriesById(int id)
        {
            var result = this.session.Load<ProductCategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesById(string id)
        {
            var result = this.session.Load<ProductCategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesByName(string name)
        {
            var result = this.session.Query<ProductCategory>().Where(x => x.Name.StartsWith(name)).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesAll()
        {
            var result = this.session.Query<ProductSubcategory>().ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/{id}")]
        public IHttpActionResult GetSubcategoriesById(int id)
        {
            var result = this.session.Load<ProductSubcategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesById(string id)
        {
            var result = this.session.Load<ProductSubcategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesByName(string name)
        {
            var result = this.session.Query<ProductSubcategory>().Where(x => x.Name.StartsWith(name)).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/category/{id}")]
        public IHttpActionResult GetSubcategoriesByCategoryId(int id)
        {
            string categoryId = "ProductCategories/" + id;
            var result = this.session.Query<ProductSubcategory>().Where(x => x.ProductCategoryId == categoryId).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/category")]
        public IHttpActionResult GetSubcategoriesByCategoryId(string id)
        {
            var result = this.session.Query<ProductSubcategory>().Where(x => x.ProductCategoryId == id).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/category")]
        public IHttpActionResult GetSubcategoriesByCategoryName(string name)
        {
            var result = this.session.Query<Subcategories_ByCategoryName.Result, Subcategories_ByCategoryName>()
                .Where(x => x.ProductCategoryName.StartsWith(name))
                .OfType<ProductSubcategory>().ToList();
            return this.Ok(result);
        }

        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesWithRelations()
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategory>()
                .Include(x => x.ProductCategoryId)
                .ToList();
            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }
            
            return this.Ok(result);
        }

        [Route("api/examples/griddata/subcategories/category")]
        public IHttpActionResult GetSubcategoriesByCategoryNameWithRelations(string name)
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<Subcategories_ByCategoryName.Result, Subcategories_ByCategoryName>()
                .Customize(a => a.Include<ProductSubcategory>(x => x.ProductCategoryId))
                .Where(x => x.ProductCategoryName.StartsWith(name))
                .OfType<ProductSubcategory>()
                .ToList();
            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }

            return this.Ok(result);
        }
    }
}
