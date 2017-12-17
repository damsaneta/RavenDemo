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

        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesAll()
        {
            var result = this.session.Query<ProductCategory>().ToList();
            return this.Ok(result);
        }

        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesByTerm(string term)
        {
            var result = this.session.Query<ProductCategory>()
                .Search(x => x.Name, term)
                .ToList();
            return this.Ok(result);
        }

        [Route("api/examples/categories/{id}")]
        public IHttpActionResult GetCategoriesById(int id)
        {
            var result = this.session.Load<ProductCategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesById(string id)
        {
            var result = this.session.Load<ProductCategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/categories")]
        public IHttpActionResult GetCategoriesByName(string name)
        {
            var result = this.session.Query<ProductCategory>().Where(x => x.Name.StartsWith(name)).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesAll()
        {
            var result = this.session.Query<ProductSubcategory>().ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesByTerm(string term)
        {
            var result = this.session.Query<ProductSubcategory, ProductSubcategories_ByName>()
                .Search(x => x.Name, term)
                .ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/{id}")]
        public IHttpActionResult GetSubcategoriesById(int id)
        {
            var result = this.session.Load<ProductSubcategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesById(string id)
        {
            var result = this.session.Load<ProductSubcategory>(id);
            return this.Ok(result);
        }

        [Route("api/examples/subcategories")]
        public IHttpActionResult GetSubcategoriesByName(string name)
        {
            var result = this.session.Query<ProductSubcategory>().Where(x => x.Name.StartsWith(name)).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/category/{id}")]
        public IHttpActionResult GetSubcategoriesByCategoryId(int id)
        {
            string categoryId = "ProductCategories/" + id;
            var result = this.session.Query<ProductSubcategory>().Where(x => x.ProductCategoryId == categoryId).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/category")]
        public IHttpActionResult GetSubcategoriesByCategoryId(string id)
        {
            var result = this.session.Query<ProductSubcategory>().Where(x => x.ProductCategoryId == id).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/subcategories/category")]
        public IHttpActionResult GetSubcategoriesByCategoryName(string name)
        {
            var result = this.session.Query<ProductSubcategories_ByProductCategoryName.Result, ProductSubcategories_ByProductCategoryName>()
                .Where(x => x.ProductCategoryName.StartsWith(name))
                .OfType<ProductSubcategory>().ToList();
            return this.Ok(result);
        }

        //---------------------------------------------------------------

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

        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesByExactMatchTermWithRelations(string exactMatchTerm)
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategory, ProductSubcategories_ByNameExactMatchTerm>()
                .Include(x => x.ProductCategoryId)
                .Search(x => x.Name, exactMatchTerm)
                .ToList();

            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }

            return this.Ok(result);
        }

        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesWithRelations()
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategory>()
                .Include(x => x.ProductCategoryId)
                .ToList();
            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }
            
            return this.Ok(result);
        }

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

        //Where + In
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

        [Route("api/examples/griddata/subcategories/{id}")]
        public IHttpActionResult GetSubcategoriesByIdWithRelations(int id)
        {
            var subcategory = this.session.Include<ProductSubcategory>(x => x.ProductCategoryId)
                .Load<ProductSubcategory>(id);
            var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
            var result = new ProductSubcategoryDto(subcategory, category.Name);
            return this.Ok(result);
        }

        [Route("api/examples/griddata/subcategories")]
        public IHttpActionResult GetSubcategoriesByIdWithRelations(string id)
        {
            var subcategory = this.session.Include<ProductSubcategory>(x => x.ProductCategoryId)
                .Load<ProductSubcategory>(id);
            var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
            var result = new ProductSubcategoryDto(subcategory, category.Name);
            return this.Ok(result);

        }

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

        [Route("api/examples/griddata/subcategories/category/{id}")]
        public IHttpActionResult GetSubcategoriesByCategoryIdWithRelations(int id)
        {
            string categoryId = "ProductCategories/" + id;
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategory>()
                .Customize(a => a.Include<ProductSubcategory>(x => x.ProductCategoryId))
                .Where(x => x.ProductCategoryId == categoryId)
                .OfType<ProductSubcategory>()
                .ToList();
            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }

            return this.Ok(result);
        }

        [Route("api/examples/griddata/subcategories/category")]
        public IHttpActionResult GetSubcategoriesByCategoryIdWithRelations(string id)
        {
            var result = new List<ProductSubcategoryDto>();
            var subcategories = this.session.Query<ProductSubcategory>()
                .Customize(a => a.Include<ProductSubcategory>(x => x.ProductCategoryId))
                .Where(x => x.ProductCategoryId == id)
                .OfType<ProductSubcategory>()
                .ToList();
            foreach (var subcategory in subcategories)
            {
                var category = this.session.Load<ProductCategory>(subcategory.ProductCategoryId);
                result.Add(new ProductSubcategoryDto(subcategory, category.Name));
            }
            return this.Ok(result);
        }


        //------------------produkty------------------


        [Route("api/examples/products")]
        public IHttpActionResult GetProductsAll()
        {
            var result = this.session.Query<Product>().Take(1024).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/products/{id}")]
        public IHttpActionResult GetProductsById(int id)
        {
            var result = this.session.Load<Product>(id);
            return this.Ok(result);
        }

        [Route("api/examples/products")]
        public IHttpActionResult GetProductsById(string id)
        {
            var result = this.session.Load<Product>(id);
            return this.Ok(result);
        }

        [Route("api/examples/products")]
        public IHttpActionResult GetProductsByName(string name)
        {
            var result = this.session.Query<Product>().Where(x => x.Name.StartsWith(name)).Take(1024).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/products/subcategories/{id}")]
        public IHttpActionResult GetProductsBySubcategoryId(int id)
        {
            string subcategoryId = "ProductSubcategories/" + id;
            var result = this.session.Query<Product>().Where(x => x.ProductSubcategoryId == subcategoryId).Take(1024).ToList();
            return this.Ok(result);
        }

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

        [Route("api/examples/products/boost")]
        public IHttpActionResult GetProductsByNameAndColorWithBoost()
        {
            var result = this.session.Query<Product, Products_ByNameAndColor>()
                .Search(x => x.Name, "Chain")
                .Search(x => x.Color, "Silver")
                .Take(1024)
                .ToList();

            return this.Ok(result);
        }

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

        [Route("api/examples/products/subcategories")]
        public IHttpActionResult GetProductsBySubcategoryId(string id)
        {
            var result = this.session.Query<Product>().Where(x => x.ProductSubcategoryId == id).Take(1024).ToList();
            return this.Ok(result);
        }

        [Route("api/examples/products/subcategories")]
        public IHttpActionResult GetProductsBySubcategoryName(string name)
        {
            var result = this.session.Query<Products_BySubcategoryName.Result, Products_BySubcategoryName>()
                .Where(x => x.ProductSubcategoryName.StartsWith(name))
                .OfType<Product>().ToList();
            return this.Ok(result);
        }

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
        //---------------------------------------------------------------

        [Route("api/examples/griddata/products")]
        public IHttpActionResult GetProductsWithRelations()
        {
            var result = new List<ProductDto>();
            RavenQueryStatistics statistics;
            var products = this.session.Query<Product>()
                .Statistics(out statistics)
                .Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(3)))
                .Include(x => x.ProductSubcategoryId).Take(1024)
                .ToList();

            //var products = this.session.Query<Product>()
            //    .Statistics(out statistics)
            //    .Customize(x => x.WaitForNonStaleResultsAsOf(new DateTime(2017, 10, 31, 10, 0, 0)))
            //    .Include(x => x.ProductSubcategoryId).Take(1024)
            //    .ToList();


            //var products = this.session.Query<Product>()
            //    .Statistics(out statistics)
            //    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
            //    .Include(x => x.ProductSubcategoryId).Take(1024)
            //    .ToList();

            if (statistics.IsStale)
            {
                //dane nie są aktualne
            }
            else
            {
              //dane są aktualne   
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

        [Route("api/examples/griddata/products/{id}")]
        public IHttpActionResult GetProductsByIdWithRelations(int id)
        {
            var product = this.session.Include<Product>(x => x.ProductSubcategoryId)
                .Load<Product>(id);
            var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
            var result = new ProductDto();
            result = subcategory == null ? new ProductDto(product, "") : new ProductDto(product, subcategory.Name);
            return this.Ok(result);
        }

        [Route("api/examples/griddata/products")]
        public IHttpActionResult GetProductsByIdWithRelations(string id)
        {
            var product = this.session.Include<Product>(x => x.ProductSubcategoryId)
                .Load<Product>(id);
            var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
            var result = new ProductDto();
            result = subcategory == null ? new ProductDto(product, "") : new ProductDto(product, subcategory.Name);
            return this.Ok(result);

        }

        [Route("api/examples/griddata/products")]
        public IHttpActionResult GetProductsByNameWithRelations(string name)
        {
            var result = new List<ProductDto>();
            var products = this.session.Query<Product>()
                .Customize(a => a.Include<Product>(x => x.ProductSubcategoryId))
                .Take(1024)
                .Where(x => x.Name.StartsWith(name))
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

        [Route("api/examples/griddata/products/subcategories/{id}")]
        public IHttpActionResult GetProductsBySubcategoryIdWithRelations(int id)
        {
            string subcategoryId = "ProductSubcategories/" + id;
            var result = new List<ProductDto>();
            var products = this.session.Query<Product>()
                .Customize(a => a.Include<Product>(x => x.ProductSubcategoryId))
                .Where(x => x.ProductSubcategoryId == subcategoryId)
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

        [Route("api/examples/griddata/products/subcategories")]
        public IHttpActionResult GetProductsBySubcategoryIdWithRelations(string id)
        {
            var result = new List<ProductDto>();
            var products = this.session.Query<Product>()
                .Customize(a => a.Include<Product>(x => x.ProductSubcategoryId))
                .Where(x => x.ProductSubcategoryId == id)
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

        //nie chce działać
        [Route("api/examples/griddata/products")]
        public IHttpActionResult GetProductsWithRelationsOrderedBySubcategoryId(bool descending)
        {
            var result = new List<ProductDto>();
            var products = this.session.Query<Product>()
                .Customize(x => x.AlphaNumericOrdering<Product>(y => y.Id))
                .Include(x => x.ProductSubcategoryId)
                .Take(1024)
                .OfType<Product>()
                .ToList();

            // .Where(x => x.ListPrice > 500)

            foreach (var product in products)
            {
                var subcategory = this.session.Load<ProductSubcategory>(product.ProductSubcategoryId);
                result.Add(subcategory == null
                    ? new ProductDto(product, "")
                    : new ProductDto(product, subcategory.Name));
            }

            return this.Ok(result);
        }


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

        [Route("api/examples/griddata/productInventories")]
        public IHttpActionResult GetProductInventoriesWithRelations()
        {
            var result = new List<ProductInventoryDto>();
            var productInventories = this.session.Query<ProductInventory>()
                .Customize(a => a.Include<ProductInventory>(x => x.ProductId))
                .Customize(a => a.Include<ProductInventory>(x => x.LocationId))
                .Take(1024)
                .ToList();
            foreach (var productInventory in productInventories)
            {
                var product = this.session.Load<Product>(productInventory.ProductId);
                var location = this.session.Load<Location>(productInventory.LocationId);
                result.Add( new ProductInventoryDto(productInventory, product.Name, location.Name));
            }

            return this.Ok(result);
        }

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


        //RAPORTY

        [Route("api/examples/raports/products/groupByProductSubcategoryId")]
        public IHttpActionResult GetProductsGroupByProductSubcategoryId()
        {
            var result = this.session.Query<Products_GroupByProductSubcategoryId.Result, Products_GroupByProductSubcategoryId>()
               .Take(1024)
               .ToList();
            return this.Ok(result);
        }

        [Route("api/examples/raports/productInventories/groupByProductId")]
        public IHttpActionResult GetProductInventoriesGroupByProductId()
        {
            var result = this.session.Query<ProductInventories_GroupByProdctId.Result, ProductInventories_GroupByProdctId>()
               .Take(1024)
               .ToList();
            return this.Ok(result);
        }

    }
}
