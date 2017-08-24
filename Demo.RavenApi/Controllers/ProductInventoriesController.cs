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
using Demo.RavenApi.Models.DataTables;
using Raven.Client;

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

            return this.Ok(new ProductInventoryDto(result, "", ""));
        }

        [ResponseType(typeof (IList<ProductInventoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductInventoryDto> request)
        {
            IQueryable<ProductInventory> prodInventory = this.session.Query<ProductInventory>()
                .Include(x => x.ProductId).Include(x => x.LocationId);

            IQueryable<ProductInventoryDto> queryDto = prodInventory.Select(inventory => new ProductInventoryDto
            {
                ProductId = inventory.ProductId,
                LocationId = inventory.LocationId,
                Shelf = inventory.Shelf,
                Bin = inventory.Bin,
                Quantity = inventory.Quantity
            });

            if (!string.IsNullOrEmpty(request.Search))
            {
                queryDto =
                    queryDto.Where(
                        x => x.ProductName.StartsWith(request.Search) || x.LocationName.StartsWith(request.Search));
            }

            switch (request.OrderColumn)
            {
                case "ProductId":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductId)
                        : queryDto.OrderByDescending(x => x.ProductId);
                    break;
                case "LocationId":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.LocationId)
                        : queryDto.OrderByDescending(x => x.LocationId);
                    break;
                case "Bin":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Bin)
                        : queryDto.OrderByDescending(x => x.Bin);
                    break;
                case "Shelf":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Shelf)
                        : queryDto.OrderByDescending(x => x.Shelf);
                    break;
                case "Quantity":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Quantity)
                        : queryDto.OrderByDescending(x => x.Quantity);
                    break;
                case "LocationName":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.LocationName)
                        : queryDto.OrderByDescending(x => x.LocationName);
                    break;
                //default:
                //    queryDto = request.OrderDirection == DtOrderDirection.ASC
                //        ? queryDto.OrderBy(x => x.ProductName)
                //        : queryDto.OrderByDescending(x => x.ProductName);
                //    break;
            }

            var result = queryDto.Take(2048).ToList();

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
