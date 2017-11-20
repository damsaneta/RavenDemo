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
using Raven.Client.Linq;

namespace Demo.RavenApi.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult Get(string id)
        {
            var result = this.session.Load<Product>(id);
            var productSubcategoryName = "";

            if (result == null)
            {
                return this.NotFound();
            }

            if (result.ProductSubcategoryId != "ProductSubcategories/")
            {
                productSubcategoryName = this.session.Load<ProductSubcategory>(result.ProductSubcategoryId).Name;
            }   

            return this.Ok(new ProductDto(result, productSubcategoryName));
        }

        [ResponseType(typeof(IList<ProductDto>))]
        public IHttpActionResult Get(DtRequest<ProductDto> request)
        {

            IRavenQueryable<Products_ByNameAndSubcategoryNameAndColorAndProductNumber.Result> indexQuery = this.session.Query<Products_ByNameAndSubcategoryNameAndColorAndProductNumber.Result,
                        Products_ByNameAndSubcategoryNameAndColorAndProductNumber>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(x => x.Name.StartsWith(request.Search) || x.ProductSubcategoryName.StartsWith(request.Search)
               || x.ProductNumber.StartsWith(request.Search) || x.Color.StartsWith(request.Search));
            }
            else if (request.SearchByColumnValues.Any())
            {
                foreach (var searchByColumnValue in request.SearchByColumnValues)
                {
                    var columnName = searchByColumnValue.Key;
                    var searchValue = searchByColumnValue.Value;
                    switch (columnName)
                    {
                        case "Name":
                            indexQuery = indexQuery.Where(x => x.Name.StartsWith(searchValue));
                            break;
                        case "ProductSubcategoryName":
                            indexQuery = indexQuery.Where(x => x.ProductSubcategoryName.StartsWith(searchValue));
                            break;
                        case "Color":
                            indexQuery = indexQuery.Where(x => x.Color.StartsWith(searchValue));
                            break;
                        case "ProductNumber":
                            indexQuery = indexQuery.Where(x => x.ProductNumber.StartsWith(searchValue));
                            break;
                        default: throw new ArgumentException("Nieznana kolumna", columnName);
                    }
                }
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "Name", request.OrderDirection == DtOrderDirection.DESC));
            List<ProductDto> result = indexQuery.ProjectFromIndexFieldsInto<ProductDto>()
                .Take(1024)
                .ToList();

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
            entity.SafetyStockLevel = productDto.SafetyStockLevel;
            entity.ReorderPoint = productDto.ReorderPoint;
            entity.ProductNumber = productDto.ProductNumber;
            entity.SellStartDate = productDto.SellStartDate;
            entity.SellEndDate = productDto.SellEndDate;
            entity.ProductSubcategoryId =  productDto.ProductSubcategoryId;
            entity.Size = productDto.Size;
            entity.SizeUnitMeasureCode = productDto.SizeUnitMeasureCode;
            entity.Weight = productDto.Weight;
            entity.WeightUnitMeasureCode = productDto.WeightUnitMeasureCode;

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
