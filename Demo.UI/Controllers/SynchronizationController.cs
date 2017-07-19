
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
            SynchronizeProductCategories();
            SynchronizeLocations();
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
                        if (sqlEntity.Name == ravenEntity.Name)
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
                            Id = "productCategories/" + sqlEntity.ID.ToString()
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
                result = JsonConvert.DeserializeObject<List<LocationDto>>(content);
            }

            using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
            {
                foreach (var sqlEntity in result)
                {
                    HttpResponseMessage response = client.GetAsync("Locations/" + sqlEntity.ID).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        var ravenEntity = JsonConvert.DeserializeObject<Model.Raven.Entities.Location>(content);
                        if (sqlEntity.Name == ravenEntity.Name)
                        {
                            ravenEntity.Name = sqlEntity.Name;
                            response = client.PutAsJsonAsync("Locations", ravenEntity).Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    else
                    {
                        var ravenEntity = new Model.Raven.Entities.Location
                        {
                            Name = sqlEntity.Name,
                            Id = "locations/" + sqlEntity.ID.ToString()
                        };
                        response = client.PostAsJsonAsync("Locations", ravenEntity).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }
    }
}