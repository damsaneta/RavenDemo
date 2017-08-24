using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.LinqApi.Model;
using Demo.LinqApi.Model.DataTables;
using Demo.Model.EF.Dtos;
using Demo.Model.EF.Entities;

namespace Demo.LinqApi.Controllers
{
    public class ProductInventoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [Route("api/productInventories/{productId}/{locationId}")]
        [ResponseType(typeof(ProductInventoryDto))]
        public IHttpActionResult Get(int productId, int locationId)
        { 
            var prodInventory = db.Set<ProductInventory>().Where(x => x.ProductId == productId && x.LocationId == locationId);

            var productInventory = prodInventory.Select(inventory => new ProductInventoryDto
            {
                ProductId = inventory.ProductId,
                LocationId = inventory.LocationId,
                Shelf = inventory.Shelf,
                Bin = inventory.Bin,
                Quantity = inventory.Quantity,
                ProductName = inventory.Product.Name,
                LocationName = inventory.Location.Name

            }).SingleOrDefault();

            if (productInventory == null)
            {
                return NotFound();
            }

            return Ok(productInventory);

        }

        [ResponseType(typeof(IList<ProductInventoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductInventoryDto> request)
        {
            IQueryable<ProductInventory>prodInventory = db.Set<ProductInventory>();

            IQueryable<ProductInventoryDto> queryDto = prodInventory.Select(inventory => new ProductInventoryDto
            {
                ProductId = inventory.ProductId,
                LocationId = inventory.LocationId,
                Shelf = inventory.Shelf,
                Bin = inventory.Bin,
                Quantity = inventory.Quantity,
                ProductName = inventory.Product.Name,
                LocationName = inventory.Location.Name
            });

            if (!string.IsNullOrEmpty(request.Search))
            {
                queryDto = queryDto.Where(x => x.ProductName.StartsWith(request.Search) || x.LocationName.StartsWith(request.Search));
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
                default:
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductName)
                        : queryDto.OrderByDescending(x => x.ProductName);
                    break;
            }

            var result = queryDto.ToList();

            return this.Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
