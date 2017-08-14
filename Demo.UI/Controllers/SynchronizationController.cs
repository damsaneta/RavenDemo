
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Demo.Model.Raven;
using System.Web.Mvc;
using Demo.Model.EF.Dtos;
using Demo.UI.Models;
using Newtonsoft.Json;

namespace Demo.UI.Controllers
{
    public class SynchronizationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //this.SynchronizeProductCategories();
            //this.SynchronizeProductSubcategories();
            //this.SynchronizeLocations();
            //this.SynchronizeUnitsMeasure();
            this.SynchronizeProducts();
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index1()
        {
            return RedirectToAction("Index");
        }

        private void SynchronizeProductCategories()
        {
            List<ProductCategoryDto> result;
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("ProductCategories").Result;
                string content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Demo.Model.EF.Dtos.ProductCategoryDto>>(content);
            }

            using (var client = new HttpClient {BaseAddress = new Uri(Consts.RavenApiRootUrl)})
            {
                foreach (var sqlEntity in result)
                {
                    HttpResponseMessage response = client.GetAsync("ProductCategories?id=ProductCategories/" + sqlEntity.Id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // update (put)
                        string content = response.Content.ReadAsStringAsync().Result;
                        var ravenEntity = JsonConvert.DeserializeObject<Demo.Model.Raven.Entities.ProductCategory>(content);
                        if (sqlEntity.Name != ravenEntity.Name)
                        {
                            ravenEntity.Name = sqlEntity.Name;
                            response = client.PutAsJsonAsync("ProductCategories", ravenEntity).Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    else
                    {
                        // insert (post)
                        var ravenEntity = new Demo.Model.Raven.Entities.ProductCategory
                        {
                            Name = sqlEntity.Name,
                            Id = "ProductCategories/" + sqlEntity.Id
                        };
                        response = client.PostAsJsonAsync("ProductCategories", ravenEntity).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }

        private void SynchronizeLocations()
        {
            List<LocationDto> result;
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("Locations").Result;
                string content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Demo.Model.EF.Dtos.LocationDto>>(content);
            }

            using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
            {
                foreach (var sqlEntity in result)
                {
                    HttpResponseMessage response = client.GetAsync("Locations?id=Locations/" + sqlEntity.Id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // update (put)
                        string content = response.Content.ReadAsStringAsync().Result;
                        var ravenEntity = JsonConvert.DeserializeObject<Demo.Model.Raven.Entities.Location>(content);
                        if (sqlEntity.Name != ravenEntity.Name)
                        {
                            ravenEntity.Name = sqlEntity.Name;
                            response = client.PutAsJsonAsync("Locations", ravenEntity).Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    else
                    {
                        // insert (post)
                        var ravenEntity = new Demo.Model.Raven.Entities.ProductCategory
                        {
                            Name = sqlEntity.Name,
                            Id = "Locations/" + sqlEntity.Id
                        };
                        response = client.PostAsJsonAsync("Locations", ravenEntity).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }

        private void SynchronizeUnitsMeasure()
        {
            List<UnitMeasureDto> result;
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("UnitsMeasure").Result;
                string content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Demo.Model.EF.Dtos.UnitMeasureDto>>(content);
            }

            using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
            {
                foreach (var sqlEntity in result)
                {
                    HttpResponseMessage response = client.GetAsync("UnitsMeasure?id=UnitsMeasures/" + sqlEntity.UnitMeasureCode).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // update (put)
                        string content = response.Content.ReadAsStringAsync().Result;
                        var ravenEntity = JsonConvert.DeserializeObject<Demo.Model.Raven.Entities.UnitMeasure>(content);
                        if (sqlEntity.Name != ravenEntity.Name)
                        {
                            ravenEntity.Name = sqlEntity.Name;
                            response = client.PutAsJsonAsync("UnitsMeasure", ravenEntity).Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    else
                    {
                        // insert (post)
                        var ravenEntity = new Demo.Model.Raven.Entities.UnitMeasure
                        {
                            Name = sqlEntity.Name,
                            UnitMeasureCode = "UnitsMeasures/" + sqlEntity.UnitMeasureCode
                        };
                        response = client.PostAsJsonAsync("UnitsMeasure", ravenEntity).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }

        private void SynchronizeProductSubcategories()
        {
            List<ProductSubcategoryDto> result;
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("ProductSubcategories").Result;
                string content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Demo.Model.EF.Dtos.ProductSubcategoryDto>>(content);
            }

            using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
            {
                foreach (var sqlEntity in result)
                {
                    HttpResponseMessage response = client.GetAsync("ProductSubcategories?id=ProductSubcategories/" + sqlEntity.Id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //(put)
                        string content = response.Content.ReadAsStringAsync().Result;
                        var ravenEntity = JsonConvert.DeserializeObject<Demo.Model.Raven.Entities.ProductSubcategory>(content);
                        if (sqlEntity.Name != ravenEntity.Name || ("ProductCategories/" + sqlEntity.ProductCategoryId) != ravenEntity.ProductCategoryId)
                        {
                            ravenEntity.Name = sqlEntity.Name;
                            ravenEntity.ProductCategoryId = "ProductCategories/" + sqlEntity.ProductCategoryId;
                            response = client.PutAsJsonAsync("ProductSubcategories", ravenEntity).Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    else
                    {
                        // insert (post)
                        var ravenEntity = new Demo.Model.Raven.Entities.ProductSubcategory()
                        {
                            Name = sqlEntity.Name,
                            Id = "ProductSubcategories/" + sqlEntity.Id,
                            ProductCategoryId = "ProductCategories/" + sqlEntity.ProductCategoryId
                        };
                        response = client.PostAsJsonAsync("ProductSubcategories", ravenEntity).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }

        private void SynchronizeProducts()
        {
            List<ProductDto> result;
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("Products").Result;
                string content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<Demo.Model.EF.Dtos.ProductDto>>(content);
            }

            using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
            {
                foreach (var sqlEntity in result)
                {
                    HttpResponseMessage response = client.GetAsync("Products?id=Products/" + sqlEntity.Id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //(put)
                        string content = response.Content.ReadAsStringAsync().Result;
                        var ravenEntity = JsonConvert.DeserializeObject<Demo.Model.Raven.Entities.Product>(content);
                        if (sqlEntity.Name != ravenEntity.Name || ("ProductSubcategories/" + sqlEntity.ProductSubcategoryId) != ravenEntity.ProductSubcategoryId)
                        {
                            ravenEntity.Name = sqlEntity.Name;
                            ravenEntity.Color = sqlEntity.Color;
                            ravenEntity.ListPrice = sqlEntity.ListPrice;
                            ravenEntity.SafetyStockLevel = sqlEntity.SafetyStockLevel;
                            ravenEntity.ReorderPoint = sqlEntity.ReorderPoint;
                            ravenEntity.ProductNumber = sqlEntity.ProductNumber;
                            ravenEntity.Size = sqlEntity.Size;
                            ravenEntity.SizeUnitMeasureCode = sqlEntity.SizeUnitMeasureCode;
                            ravenEntity.Weight = sqlEntity.Weight;
                            ravenEntity.WeightUnitMeasureCode = sqlEntity.WeightUnitMeasureCode;
                            ravenEntity.SellStartDate = sqlEntity.SellStartDate;
                            ravenEntity.SellEndDate = sqlEntity.SellEndDate;
                            ravenEntity.ProductSubcategoryId = "ProductSubcategories/" + sqlEntity.ProductSubcategoryId;
                            response = client.PutAsJsonAsync("Products", ravenEntity).Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    else
                    {
                        // insert (post)
                        var ravenEntity = new Demo.Model.Raven.Entities.Product()
                        {
                            Name = sqlEntity.Name,
                            Id = "Products/" + sqlEntity.Id,
                            Color = sqlEntity.Color,
                            ListPrice = sqlEntity.ListPrice,
                            SafetyStockLevel = sqlEntity.SafetyStockLevel,
                            ReorderPoint = sqlEntity.ReorderPoint,
                            ProductNumber = sqlEntity.ProductNumber,
                            Size = sqlEntity.Size,
                            SizeUnitMeasureCode = sqlEntity.SizeUnitMeasureCode,
                            Weight = sqlEntity.Weight,
                            WeightUnitMeasureCode = sqlEntity.WeightUnitMeasureCode,
                            SellStartDate = sqlEntity.SellStartDate,
                            SellEndDate = sqlEntity.SellEndDate,
                            ProductSubcategoryId = "ProductSubcategories/" + sqlEntity.ProductSubcategoryId
                        };
                        response = client.PostAsJsonAsync("Products", ravenEntity).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }

    }
}