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
using Demo.RavenApi.Models.DataTables;
using Raven.Client;
using Raven.Client.Linq;

namespace Demo.RavenApi.Controllers
{
    public class ProductInventoriesController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof (ProductInventoryDto))]
        public IHttpActionResult Get(string id)
        {
            var result = this.session.Load<ProductInventory>(id);
            if (result == null)
            {
                return this.NotFound();
            }

            var productName = this.session.Load<Product>(result.ProductId).Name;
            var locationName = this.session.Load<Location>(result.LocationId).Name;

            return this.Ok(new ProductInventoryDto(result, productName, locationName));
        }

        [ResponseType(typeof (IList<ProductInventoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductInventoryDto> request)
        {
            IRavenQueryable<ProductInventories_ByProductNameAndLocationName.Result> indexQuery = this.session.Query<ProductInventories_ByProductNameAndLocationName.Result,
                       ProductInventories_ByProductNameAndLocationName>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(x => x.ProductName.StartsWith(request.Search) || x.LocationName.StartsWith(request.Search));
            }
            else if (request.SearchByColumnValues.Any())
            {
                foreach (var searchByColumnValue in request.SearchByColumnValues)
                {
                    var columnName = searchByColumnValue.Key;
                    var searchValue = searchByColumnValue.Value;
                    switch (columnName)
                    {
                        case "ProductName":
                            indexQuery = indexQuery.Where(x => x.ProductName.StartsWith(searchValue));
                            break;
                        case "LocationName":
                            indexQuery = indexQuery.Where(x => x.LocationName.StartsWith(searchValue));
                            break;
                        default: throw new ArgumentException("Nieznana kolumna", columnName);
                    }
                }
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "ProductName", request.OrderDirection == DtOrderDirection.DESC));
            List<ProductInventoryDto> result = indexQuery.ProjectFromIndexFieldsInto<ProductInventoryDto>()
                .Take(1024)
                .ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Post([FromBody] ProductInventoryDto productInventoryDto)
        {
            var entity = new ProductInventory(productInventoryDto);
            this.session.Store(entity);
            this.session.SaveChanges();
            return this.Ok(entity);
        }

        public IHttpActionResult Put([FromBody] ProductInventoryDto productInventoryDto)
        {
            ProductInventory entity = this.session.Load<ProductInventory>(productInventoryDto.ProductId + "_" + productInventoryDto.LocationId);
            if (entity == null)
            {
                return this.NotFound();
            }

            entity.Bin = productInventoryDto.Bin;
            entity.Quantity = productInventoryDto.Quantity;
            entity.Shelf = productInventoryDto.Shelf;
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
