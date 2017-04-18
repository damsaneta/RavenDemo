using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Tests.Api.ApiTests.ProductCategories
{
    [TestFixture]
    public class ProductCategoriesTests
    {
        [Test]
        public void Get_all()
        {
            using (var client = new HttpClient {BaseAddress = new Uri(Consts.SqlApiRootUrl)})
            {
                HttpResponseMessage response = client.GetAsync("ProductCategories").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetAll_json.Trim());
            }
        }

        [Test]
        public void Get_by_id()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("ProductCategories/1").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetById_json.Trim());
            }
        }

        [Test]
        public void Get_by_id_not_found()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("ProductCategories/0").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Test]
        public void Get_by_name()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("ProductCategories?name=c").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetByName_json.Trim());
            }
        }

        [Test]
        public void Get_by_name_not_found()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("ProductCategories?name=x").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be("[]");
            }
        }
    }
}
