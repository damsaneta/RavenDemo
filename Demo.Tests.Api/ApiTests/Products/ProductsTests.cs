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
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        //[TestCase(Consts.RavenApiRootUrl)]
        public void Get_all(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                HttpResponseMessage response = client.GetAsync("Products").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                var r = content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim();
                var r2 = ProductsFiles.GetAll_json.Replace("\"", "").Trim();
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
                .Should().Be(ProductsFiles.GetAll_json.Replace("\"", "").Trim());
                //content.Should().Be(ProductsFiles.GetAll_json.Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_Id(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                HttpResponseMessage response = client.GetAsync("Products/1").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(ProductsFiles.GetByProductId_json.Trim());

            }
        }

        //[Test]
        //public void Get_by_Id()
        //{
        //    using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
        //    {
        //        HttpResponseMessage response = client.GetAsync("ProductSubcategories?id=ProductSubcategories/1").Result;
        //        response.Should().NotBeNull();
        //        response.IsSuccessStatusCode.Should().BeTrue();
        //        response.StatusCode.Should().Be(HttpStatusCode.OK);
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        var t =
        //            content.Replace("ProductSubcategories/", "")
        //                .Replace("ProductCategories/", "")
        //                .Replace("\"", "")
        //                .Trim();
        //        content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
        //      .Should().Be(ProductsFiles.GetByProductId_json.Replace("\"", "").Trim());

        //    }
        //}

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_Id_not_found(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                HttpResponseMessage response = client.GetAsync("Products/0").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        //[Test]
        //public void Get_by_Id_not_found()
        //{
        //    using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
        //    {
        //        HttpResponseMessage response = client.GetAsync("ProductSubcategories?id=ProductSubcategories/0").Result;
        //        response.Should().NotBeNull();
        //        response.IsSuccessStatusCode.Should().BeFalse();
        //        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //    }
        //}

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        //[TestCase(Consts.RavenApiRootUrl)]
        public void Get_by_ProductName(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl("Blade");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
               .Should().Be(ProductsFiles.Get_by_ProductName_json.Replace("\"", "").Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        //[TestCase(Consts.RavenApiRootUrl)]
        public void Get_by_ProductSubcategory(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl("Road Bikes");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var content = response.Content.ReadAsStringAsync().Result;
                var res = content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim();
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
               .Should().Be(ProductsFiles.Get_by_ProductSubcategory_json.Replace("\"", "").Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        //[TestCase(Consts.RavenApiRootUrl)]
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
        //[TestCase(Consts.RavenApiRootUrl)]
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
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
               .Should().Be(ProductsFiles.GetAllOrderedByNameDesc_json.Replace("\"", "").Trim());
            }

        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        //[TestCase(Consts.RavenApiRootUrl)]
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
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
                .Should().Be(ProductsFiles.GetAllOrderedByNameAsc_json.Replace("\"", "").Trim());

            }

        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        //[TestCase(Consts.RavenApiRootUrl)]
        public void Get_all_ordered_by_subcategoryName_desc(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 1, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
               .Should().Be(ProductsFiles.Get_all_ordered_by_subcategoryName_desc_json.Replace("\"", "").Trim());
            }

        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        //[TestCase(Consts.RavenApiRootUrl)]
        public void Get_all_ordered_by_subcategoryName_asc(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 1);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Products/", "").Replace("ProductSubcategories/", "").Replace("\"", "").Trim()
               .Should().Be(ProductsFiles.Get_all_ordered_by_subcategoryName_asc_json.Replace("\"", "").Trim());

            }
        }

        private string BuildDtUrl(string search = null, int? orderColumn = null, string orderDirection = null)
        {
            var url = "Products?draw=1";
            url += "&" + WebUtility.UrlEncode("columns[0][name]") + "=Name";
            url += "&" + WebUtility.UrlEncode("columns[1][name]") + "=ProductSubcategoryName";
            //url += "&" + WebUtility.UrlEncode("columns[2][name]") + "=Id";
            url += "&" + WebUtility.UrlEncode("columns[2][name]") + "=ProductSubcategoryId";
            url += "&" + WebUtility.UrlEncode("columns[3][name]") + "=Color";
            url += "&" + WebUtility.UrlEncode("columns[4][name]") + "=ProductNumber";
            url += "&" + WebUtility.UrlEncode("columns[5][name]") + "=ListPrice";
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
