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
                var url = this.BuildDtUrl("c");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetByName_json.Trim());
            }
        }

        [Test]
        public void Get_by_name_empty()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl("x");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be("[]");
            }
        }

        [Test]
        public void Get_all_ordered_by_id_ascending()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl(orderColumn: 1);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetAllOrderByIdAsc_json.Trim());
            }
        }

        [Test]
        public void Get_all_ordered_by_id_descending()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl(orderColumn: 1, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetAllOrderByIdDesc_json.Trim());
            }
        }

        [Test]
        public void Get_all_ordered_by_name_ascending()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl(orderColumn: 0);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetAllOrderByNameAsc_json.Trim());
            }
        }

        [Test]
        public void Get_all_ordered_by_name_descending()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl(orderColumn: 0, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductCategoriesFiles.GetAllOrderByNameDesc_json.Trim());
            }
        }

        private string BuildDtUrl(string search = null, int? orderColumn = null, string orderDirection = null)
        {
            var url = Consts.SqlApiRootUrl + "ProductCategories?draw=1";
            url += "&" + WebUtility.UrlEncode("columns[0][name]") + "=Name";
            url += "&" + WebUtility.UrlEncode("columns[1][name]") + "=ID";
            if (orderColumn.HasValue)
            {
                url += "&" + WebUtility.UrlEncode("order[0][column]") + "=" + orderColumn;
            }

            if (!string.IsNullOrEmpty(orderDirection))
            {
                url += "&" + WebUtility.UrlEncode("order[0][dir]") + "=" + orderDirection;
            }
            if (!string.IsNullOrEmpty(search))
            {
                url += "&" + WebUtility.UrlEncode("search[value]") + "=" + search;
            }

            url += "&_" + DateTime.Now.Ticks;
            Console.WriteLine(url);
            return url;
        }
    }
}
