using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Demo.Tests.Api.ApiTests.ProductCategories;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Tests.Api.ApiTests.ProductSubcategories
{
    [TestFixture]
    public class ProductSubcategoriesTests
    {
        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        public void Get_all(string url)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(url) })
            {
                HttpResponseMessage response = client.GetAsync("ProductSubcategories").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductSubcategoriesFiles.GetAll_json.Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        public void Get_by_id(string url)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(url)})
            {
                HttpResponseMessage response = client.GetAsync("ProductSubcategories/1").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductSubcategoriesFiles.GetBySubcategoryId_json.Trim());

            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        public void Get_by_id_not_found(string url)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(url)})
            {
                HttpResponseMessage response = client.GetAsync("ProductSubcategories/0").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
 
    }
}
