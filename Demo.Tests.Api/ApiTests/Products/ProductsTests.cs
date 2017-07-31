using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Tests.Api.ApiTests.Products
{
    [TestFixture]
    public class ProductsTests
    {
        [Test]
      //  [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_all(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                HttpResponseMessage response = client.GetAsync("Products").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
                .Should().Be(ProductsFiles.GetAll_json.Replace("\"", "").Trim());
                //content.Should().Be(ProductsFiles.GetAll_json.Trim());
            }
        }

        private string BuildDtUrl(string search = null, int? orderColumn = null, string orderDirection = null)
        {
            var url = "Products?draw=1";
            url += "&" + WebUtility.UrlEncode("columns[0][name]") + "=Name";
            url += "&" + WebUtility.UrlEncode("columns[1][name]") + "=ProductSubcategoryName";
            url += "&" + WebUtility.UrlEncode("columns[2][name]") + "=Id";
            url += "&" + WebUtility.UrlEncode("columns[3][name]") + "=ProductSubcategoryId";
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
