using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.Model.Raven.Dtos;
using Demo.Model.Raven.Entities;
using Demo.RavenApi.Infrastructure;
using Demo.RavenApi.Infrastructure.Indexes;
using Demo.RavenApi.Models;
using Demo.RavenApi.Models.DataTables;
using Raven.Client;
using Raven.Client.Linq;

namespace Demo.RavenApi.Controllers
{
    public class ProductsGroupBySubcategoryRaportController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        //[ResponseType(typeof(ProductDto))]
        //public IHttpActionResult Get(string id)
        //{
        //    var result = this.session.Load<Product>(id);
        //    var productSubcategoryName = "";

        //    if (result == null)
        //    {
        //        return this.NotFound();
        //    }

        //    if (result.ProductSubcategoryId != "ProductSubcategories/")
        //    {
        //        productSubcategoryName = this.session.Load<ProductSubcategory>(result.ProductSubcategoryId).Name;
        //    }   

        //    return this.Ok(new ProductDto(result, productSubcategoryName));
        //}

        [ResponseType(typeof(IList<ProductsGroupBySubcategoryRaportDto>))]
        public IHttpActionResult Get(DtRequest<ProductsGroupBySubcategoryRaportDto> request)
        {

            IRavenQueryable<Products_GroupByProductSubcategoryId.Result> indexQuery = this.session.Query<Products_GroupByProductSubcategoryId.Result,
                        Products_GroupByProductSubcategoryId>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(x => x.ProductSubcategoryName.StartsWith(request.Search));
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "ProductSubcategoryName", request.OrderDirection == DtOrderDirection.DESC));
            List<ProductsGroupBySubcategoryRaportDto> result = indexQuery.ProjectFromIndexFieldsInto<ProductsGroupBySubcategoryRaportDto>()
                .Take(1024)
                .ToList();

            return this.Ok(result);
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
