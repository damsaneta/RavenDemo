﻿using System;
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
            var result = this.session.Load<ProductSubcategory>(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ProductSubcategoryDto(result));
        }

        [ResponseType(typeof (IList<ProductSubcategoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductSubcategoryDto> request)
        {
            IQueryable<ProductCategory> productCat = this.session.Query<ProductCategory>();
            IQueryable<ProductSubcategory> prodSubCat = this.session.Query<ProductSubcategory>();

            IQueryable<ProductSubcategoryDto> queryDto = productCat.Join(prodSubCat,
                category => category.Id,
                subcategory => subcategory.ProductCategoryId,
                (category, subcategory) => new ProductSubcategoryDto
                {
                    Id = subcategory.Id,
                    Name = subcategory.Name,
                    ProductCategoryId = category.Id,
                    ProductCategoryName = category.Name

                }
            );

            if (!string.IsNullOrEmpty(request.Search))
            {
                queryDto =
                    queryDto.Where(
                        x => x.Name.StartsWith(request.Search) || x.ProductCategoryName.StartsWith(request.Search));
            }

            switch (request.OrderColumn)
            {
                case "Id":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.Id);
                    break;
                case "ProductCategoryId":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.ProductCategoryId);
                    break;
                case "Name":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Name)
                        : queryDto.OrderByDescending(x => x.Name);
                    break;
                default:
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ProductCategoryName)
                        : queryDto.OrderByDescending(x => x.ProductCategoryName);
                    break;
            }

            var result = queryDto.ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Post([FromBody]ProductSubcategoryDto productSubcategoryDto)
        {
            var entity = new ProductSubcategory(productSubcategoryDto);
            this.session.Store(entity);
            this.session.SaveChanges();
            return this.Ok(entity.Id);
        }

        public IHttpActionResult Put([FromBody]ProductSubcategoryDto productSubcategoryDto)
        {
            ProductSubcategory entity = this.session.Load<ProductSubcategory>(productSubcategoryDto.Id);
            if (entity == null)
            {
                return this.NotFound();
            }

            entity.Name = productSubcategoryDto.Name;
            entity.ProductCategoryName = productSubcategoryDto.ProductCategoryName;
            entity.ProductCategoryId = productSubcategoryDto.ProductCategoryId;
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