using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Tests.Api.ApiTests.ProductSubcategories
{
    [TestFixture]
    public class ProductSubcategoriesTests
    {
        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_all(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
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
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_id(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root)})
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
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_id_not_found(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root)})
            {
                HttpResponseMessage response = client.GetAsync("ProductSubcategories/0").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
        
        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_subcategoryName_or_categoryName(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl("bike");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductSubcategoriesFiles.Get_by_subcategoryName_or_categoryName_json.Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_name_empty(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl("z");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be("[]");
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_all_ordered_by_name_desc(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 0, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductSubcategoriesFiles.GetAllOrderedByNameDesc_json.Trim());
            }

        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_all_ordered_by_name_asc(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 0);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductSubcategoriesFiles.GetAllOrderedByNameAsc_json.Trim());

            }

        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_all_ordered_by_categoryName_desc(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 1, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductSubcategoriesFiles.GetAllOrderedByCategoryNameDesc_json.Trim());
            }

        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_all_ordered_by_categoryName_asc(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 1);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductSubcategoriesFiles.GetAllOrderedByCategoryNameAsc_json.Trim());

            }

        }

        private string BuildDtUrl(string search = null, int? orderColumn = null, string orderDirection = null)
        {
            var url = "ProductSubcategories?draw=1";
            url += "&" + WebUtility.UrlEncode("columns[0][name]") + "=Name";
            url += "&" + WebUtility.UrlEncode("columns[1][name]") + "=ProductCategoryName";
            url += "&" + WebUtility.UrlEncode("columns[2][name]") + "=ID";
            url += "&" + WebUtility.UrlEncode("columns[3][name]") + "=ProductCategoryID";
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
