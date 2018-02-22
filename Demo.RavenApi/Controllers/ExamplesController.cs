using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Demo.Model.Raven.Dtos;
using Demo.Model.Raven.Entities;
using Demo.RavenApi.Infrastructure;
using Demo.RavenApi.Infrastructure.Indexes;
using Demo.RavenApi.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace Demo.RavenApi.Controllers
{
    public class ExamplesController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        //Przykład 1
        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesByTerm(string term)
        {
            var result = this.session.Query<ProductSubcategory, ProductSubcategories_ByName>()
                .Search(x => x.Name, term)
                .ToList();
            return this.Ok(result);
        }

        //Przykład 2
        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesByName(string name)
        {
            var result = this.session.Query<ProductCategory, ProductCategories_ByName>()
                .Where(x => x.Name.StartsWith(name))
                .ToList();
            return this.Ok(result);
        }

        //Przykład 3 - Where + In
        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesByCategoryNameByWhereAndInWithRelations(string firstTerm, string secondTerm)
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategories_ByProductCategoryName.Result, ProductSubcategories_ByProductCategoryName>()
                .Customize(a => a.Include<ProductSubcategory>(x => x.ProductCategoryId))
                .Where(x => x.ProductCategoryName.In(firstTerm, secondTerm))
                .OfType<ProductSubcategory>()
                .ToList();
            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }

            return this.Ok(result);
        }

        //Przykład 4 
        [Route("api/examples/griddata/products")]
        public IHttpActionResult GetProductsWithRelationsOrderedByListPriceDescending(int listPriceMoreThan)
        {
            var result = new List<ProductDto>();
            var products = this.session.Query<Product, Products_ByListPrice>()
                .Include(x => x.ProductSubcategoryId).Take(1024)
                .Where(x => x.ListPrice > 500)
                .OrderByDescending(x => x.ListPrice)
                .ToList();

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                    ? new ProductDto(product, "")
                    : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }

        //Przykład 5
        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesByNameWithRelations(string name)
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategory>()
                .Customize(a => a.Include<ProductSubcategory>(x => x.ProductCategoryId))
                .Where(x => x.Name.StartsWith(name))
                .ToList();

            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }

            return this.Ok(result);
        }

        //Przykład 6
        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesByTermWithRelations(string term)
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategory, ProductSubcategories_ByName>()
                .Include(x => x.ProductCategoryId)
                .Search(x => x.Name, term)
                .ToList();

            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }

            return this.Ok(result);
        }

        //Przykład 7
        [Route("api/examples/products")]
        public IHttpActionResult GetProductsByNameAndColor(string name, string color)
        {
            var result = this.session.Query<Product, Products_ByNameAndColor>()
                .Search(x => x.Name, name, options: SearchOptions.Or)
                .Search(x => x.Color, color)
                .Take(1024)
                .ToList();

            return this.Ok(result);
        }

        //Przykład 8 
        [Route("api/examples/products/boost")]
        public IHttpActionResult GetProductsByNameAndColorWithBoost()
        {
            var result = this.session.Query<Product, Products_ByNameAndColor>()
                .Search(x => x.Name, "Chain", options: SearchOptions.Or, boost: 15)
                .Search(x => x.Color, "Silver", boost: 5)
                .Take(1024)
                .ToList();

            return this.Ok(result);
        }

        //Przykład 9
        [Route("api/examples/products/searchOption")]
        public IHttpActionResult GetProductsByNameAndColorWithSearchOption()
        {
            var result = this.session.Query<Product, Products_ByNameAndColor>()
                .Search(x => x.Name, "Chain")
                .Search(x => x.Color, "Silver", options: SearchOptions.Not | SearchOptions.And)
                .Take(1024)
                .ToList();

            return this.Ok(result);
        }

        //Przykład 10
        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesByIdWithRelations(string id)
        {
            var subcategory = this.session.Include<ProductSubcategory>(x => x.ProductCategoryId)
                .Load<ProductSubcategory>(id);
            var category = this.session.Load<ProductCategory>
                (subcategory.ProductCategoryId);
        
            var result = new ProductSubcategoryDto(subcategory, category.Name);

            return this.Ok(result);

        }

        //Przykład 11
        [Route("api/examples/griddata/subcategories/category")]
        public IHttpActionResult GetSubcategoriesByCategoryNameWithRelations(string name)
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategories_ByProductCategoryName.Result, ProductSubcategories_ByProductCategoryName>()
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

        //Przykład 12
        [Route("api/examples/products/byNameAndColorAndProductNumber")]
        public IHttpActionResult GetProductsByNameAndColorAndProductNumber()
        {
            var result = this.session.Query<Product, Products_ByNameAndColorAndProductNumber>()
                .Select(x => new
                {
                    Name = x.Name,
                    ProductNumber = x.ProductNumber,
                    Color = x.Color

                }).ToList();

            return this.Ok(result);
        }

        // Przykład 13
        [Route("api/examples/products/ByNameAndSubcategoryNameAndColorAndProductNumber")]
        public IHttpActionResult GetProductsByNameAndSubcategoryNameAndColorAndProductNumber(string search)
        {
            IRavenQueryable<Products_ByNameAndSubcategoryNameAndColorAndProductNumber.Result> indexQuery = this.session.Query<Products_ByNameAndSubcategoryNameAndColorAndProductNumber.Result,
                        Products_ByNameAndSubcategoryNameAndColorAndProductNumber>();

            indexQuery = indexQuery.Where(x => x.Name.StartsWith(search) || x.ProductSubcategoryName.StartsWith(search)
               || x.ProductNumber.StartsWith(search) || x.Color.StartsWith(search));
  
            List<ProductDto> result = indexQuery.ProjectFromIndexFieldsInto<ProductDto>()
                .Take(1024)
                .ToList();

            return this.Ok(result);
        }

        //Przykład 14
        [Route("api/examples/products/subcategories")]
        public IHttpActionResult GetProductsBySubcategoryName(string name)
        {
            var result = this.session.Query<Products_BySubcategoryName.Result, Products_BySubcategoryName>()
                .Where(x => x.ProductSubcategoryName.StartsWith(name))
                .OfType<Product>().ToList();
            return this.Ok(result);
        }

        //Przykład 15
        [Route("api/examples/griddata/productInventories")]
        public IHttpActionResult GetProductInventoriesWithRelations(string place)
        {
            var result = new List<ProductInventoryDto>();
            var productInventories = this.session.Query<ProductInventories_ByShelfAndBin.Result, ProductInventories_ByShelfAndBin>()
                .Customize(a => a.Include<ProductInventory>(x => x.ProductId))
                .Customize(a => a.Include<ProductInventory>(x => x.LocationId))
                .Take(1024)
                .Where(x => x.Place == place)
                .OfType<ProductInventory>()
                .ToList();

            foreach (var productInventory in productInventories)
            {
                var product = this.session.Load<Product>(productInventory.ProductId);
                var location = this.session.Load<Location>(productInventory.LocationId);
                result.Add(new ProductInventoryDto(productInventory, product.Name, location.Name));
            }

            return this.Ok(result);
        }

        //Przykład 16
        [Route("api/examples/griddata/products/subcategories")]
        public IHttpActionResult GetProductsBySubcategoryNameWithRelations(string name)
        {
            var result = new List<ProductDto>();
            var products = this.session.Query<Products_BySubcategoryName.Result, Products_BySubcategoryName>()
                .Customize(a => a.Include<Product>(x => x.ProductSubcategoryId))
                .Where(x => x.ProductSubcategoryName.StartsWith(name))
                .Take(1024)
                .OfType<Product>()
                .ToList();

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                  ? new ProductDto(product, "")
                  : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }

        //Przykład 17
        [Route("api/examples/griddata/products")]
        public IHttpActionResult GetProductsBySellStartYear(int year)
        {
            var result = new List<ProductDto>();
            var products = this.session.Query<Products_BySellStartDate.Result, Products_BySellStartDate>()
                .Customize(a => a.Include<Product>(x => x.ProductSubcategoryId))
                .Where(x => x.YearOfSell == year)
                .Take(1024)
                .OfType<Product>()
                .ToList();

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                  ? new ProductDto(product, "")
                  : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }

        //Przykład 18
        [Route("api/examples/raports/products/groupByProductSubcategoryId")]
        public IHttpActionResult GetProductsGroupByProductSubcategoryId()
        {
            var result = this.session.Query<Products_GroupByProductSubcategoryId.Result, Products_GroupByProductSubcategoryId>()
               .Take(1024)
               .ToList();
            return this.Ok(result);
        }

        //Przykład 19
        [Route("api/examples/raports/productInventories/groupByProductId")]
        public IHttpActionResult GetProductInventoriesGroupByProductId()
        {
            var result = this.session.Query<ProductInventories_GroupByProdctId.Result, ProductInventories_GroupByProdctId>()
               .Take(1024)
               .ToList();
            return this.Ok(result);
        }

        // IsStale
        [Route("api/examples/griddata/products/IsStale")]
        public IHttpActionResult CheckStaleData()
        {
            var result = new List<ProductDto>();
            RavenQueryStatistics statistics;

            var products = this.session.Query<Product>()
            .Statistics(out statistics)
            .Include(x => x.ProductSubcategoryId).Take(1024)
            .ToList();

            if (statistics.IsStale)
            {
                //dane nieaktualne
            }
            else
            {
                //dane aktualne   
            }

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                    ? new ProductDto(product, "")
                    : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }

        // WaitForNonStaleResultsAsOfNow 
        [Route("api/examples/griddata/products/WaitForNonStaleResultsAsOfNow")]
        public IHttpActionResult WaitForNonStaleResultsAsOfNow()
        {
            var result = new List<ProductDto>();
            RavenQueryStatistics statistics;

            var products = this.session.Query<Product>()
                .Statistics(out statistics)
                .Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(3)))
                .Include(x => x.ProductSubcategoryId).Take(1024)
                .ToList();

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                    ? new ProductDto(product, "")
                    : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }

        // WaitForNonStaleResultsAsOf
        [Route("api/examples/griddata/products/WaitForNonStaleResultsAsOf")]
        public IHttpActionResult WaitForNonStaleResultsAsOf()
        {
            var result = new List<ProductDto>();
            RavenQueryStatistics statistics;

            var products = this.session.Query<Product>()
                .Statistics(out statistics)
                .Customize(x => x.WaitForNonStaleResultsAsOf(new DateTime(2017, 10, 31, 10, 0, 0)))
                .Include(x => x.ProductSubcategoryId).Take(1024)
                .ToList();

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                    ? new ProductDto(product, "")
                    : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }

        // WaitForNonStaleResultsAsOfLastWrite
        [Route("api/examples/griddata/products/WaitForNonStaleResultsAsOfLastWrite")]
        public IHttpActionResult WaitForNonStaleResultsAsOfLastWrite()
        {
            var result = new List<ProductDto>();
            RavenQueryStatistics statistics;

            var products = this.session.Query<Product>()
                .Statistics(out statistics)
                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                .Include(x => x.ProductSubcategoryId).Take(1024)
                .ToList();

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                    ? new ProductDto(product, "")
                    : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }       

    }
}
