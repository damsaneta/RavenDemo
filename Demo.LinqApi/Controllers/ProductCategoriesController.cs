﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.LinqApi.Model.DataTables;
using Demo.Model.Dtos;
using Demo.Model.Entities;

namespace Demo.LinqApi.Controllers
{
    public class ProductCategoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [ResponseType(typeof(ProductCategoryDto))]
        public IHttpActionResult Get(int id)
        {
            var productCategory = db.Set<ProductCategory>()
                .Where(x => x.ID == id)
                .Select(x => new ProductCategoryDto
                {
                    ID = x.ID,
                    Name = x.Name
                }).SingleOrDefault();
            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(productCategory);
        }

        [ResponseType(typeof(IList<ProductCategoryDto>))]
        public IHttpActionResult Get(DtRequest<ProductCategoryDto> request)
        {
            IQueryable<ProductCategory> query = db.Set<ProductCategory>();
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.StartsWith(request.Search));
            }

            IQueryable<ProductCategoryDto> queryDto = query.Select(x => new ProductCategoryDto
            {
                ID = x.ID,
                Name = x.Name
            });
            switch (request.OrderColumn)
            {
                case "ID":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ID)
                        : queryDto.OrderByDescending(x => x.ID);
                    break;
                default:
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Name)
                        : queryDto.OrderByDescending(x => x.Name);
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