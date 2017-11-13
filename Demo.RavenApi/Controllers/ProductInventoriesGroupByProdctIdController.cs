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
    public class ProductInventoriesGroupByProdctIdController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(IList<ProductInventoriesGroupByProdctIdDto>))]
        public IHttpActionResult Get(DtRequest<ProductInventoriesGroupByProdctIdDto> request)
        {

            IRavenQueryable<ProductInventories_GroupByProdctId.Result> indexQuery = this.session.Query<ProductInventories_GroupByProdctId.Result,
                        ProductInventories_GroupByProdctId>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(x => x.ProductName.StartsWith(request.Search));
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "ProductName", request.OrderDirection == DtOrderDirection.DESC));
            List<ProductInventoriesGroupByProdctIdDto> result = indexQuery.ProjectFromIndexFieldsInto<ProductInventoriesGroupByProdctIdDto>()
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
