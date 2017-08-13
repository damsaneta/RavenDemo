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
using Demo.RavenApi.Models;
using Demo.RavenApi.Models.DataTables;
using Raven.Client;

namespace Demo.RavenApi.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult Get(string id)
        {
            var result = this.session.Load<Product>(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ProductDto(result, "TODO"));
        }

        [ResponseType(typeof(IList<ProductDto>))]
        public IHttpActionResult Get(DtRequest<ProductDto> request)
        {
            IQueryable<Product> products = this.session.Query<Product>();
            IQueryable<ProductSubcategory> prodSubCat = this.session.Query<ProductSubcategory>();

            IQueryable<ProductDto> queryDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductNumber = product.ProductNumber,
                Color = product.Color,
                ListPrice = product.ListPrice,
                ProductSubcategoryId = product.ProductSubcategoryId
            });

            if (!string.IsNullOrEmpty(request.Search))
            {
                queryDto = queryDto.Where(x => x.Name.StartsWith(request.Search) || x.ProductSubcategoryName.StartsWith(request.Search)
               || x.ProductNumber.StartsWith(request.Search) || x.Color.StartsWith(request.Search));
            }

            switch (request.OrderColumn)
            {
                case "Id":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.Id);
                    break;
                case "ProductSubcategoryId":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.ProductSubcategoryId);
                    break;
                case "ProductSubcategoryName":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductSubcategoryName)
                        : queryDto.OrderByDescending(x => x.ProductSubcategoryName);
                    break;
                case "Color":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Color)
                        : queryDto.OrderByDescending(x => x.Color);
                    break;
                case "ProductNumber":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductNumber)
                        : queryDto.OrderByDescending(x => x.ProductNumber);
                    break;
                case "ListPrice":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ListPrice)
                        : queryDto.OrderByDescending(x => x.ListPrice);
                    break;
                default:
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Name)
                        : queryDto.OrderByDescending(x => x.Name);
                    break;
            }

            var result = queryDto.Take(1024).ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Post([FromBody]ProductDto productDto)
        {
            var entity = new Product(productDto);
            this.session.Store(entity);
            this.session.SaveChanges();
            return this.Ok(entity.Id);
        }

        public IHttpActionResult Put([FromBody]ProductDto productDto)
        {
            Product entity = this.session.Load<Product>(productDto.Id);
            if (entity == null)
            {
                return this.NotFound();
            }
            entity.Name = productDto.Name;
            entity.Color = productDto.Color;
            entity.ListPrice = productDto.ListPrice;
            entity.ProductNumber = productDto.ProductNumber;
            entity.ProductSubcategoryId =  productDto.ProductSubcategoryId;
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
