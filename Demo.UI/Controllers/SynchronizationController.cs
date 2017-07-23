
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
            //SynchronizeProductCategories();
            //SynchronizeLocations();
           // SynchronizeUnitsMeasure();
            this.SynchronizeProductSubcategories();
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
                    HttpResponseMessage response = client.GetAsync("ProductCategories/" + sqlEntity.ID).Result;
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
                            Id = sqlEntity.ID.ToString()
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
                    HttpResponseMessage response = client.GetAsync("Locations/" + sqlEntity.ID).Result;
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
                            Id = sqlEntity.ID.ToString()
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
                    HttpResponseMessage response = client.GetAsync("UnitsMeasure/" + sqlEntity.UnitMeasureCode).Result;
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
                            UnitMeasureCode = sqlEntity.UnitMeasureCode.ToString()
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
                    HttpResponseMessage response = client.GetAsync("ProductSubcategories/" + sqlEntity.ID).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // update (put)
                        //string content = response.Content.ReadAsStringAsync().Result;
                        //var ravenEntity = JsonConvert.DeserializeObject<Demo.Model.Raven.Entities.UnitMeasure>(content);
                        //if (sqlEntity.Name != ravenEntity.Name)
                        //{
                        //    ravenEntity.Name = sqlEntity.Name;
                        //    response = client.PutAsJsonAsync("UnitsMeasure", ravenEntity).Result;
                        //    response.EnsureSuccessStatusCode();
                        //}
                    }
                    else
                    {
                        // insert (post)
                        var ravenEntity = new Demo.Model.Raven.Entities.ProductSubcategory()
                        {
                            Name = sqlEntity.Name,
                            Id = sqlEntity.ID.ToString(),
                            ProductCategoryId = sqlEntity.ProductCategoryID.ToString()
                        };
                        response = client.PostAsJsonAsync("ProductSubcategories", ravenEntity).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }
    }
}