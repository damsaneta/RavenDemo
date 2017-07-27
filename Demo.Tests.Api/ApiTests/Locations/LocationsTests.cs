using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Tests.Api.ApiTests.Locations
{
    [TestFixture]
    public class LocationsTests
    {
        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        [TestCase(Consts.RavenApiRootUrl)]
        public void Get_all(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                HttpResponseMessage response = client.GetAsync("Locations").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Locations/", "").Replace("\"", "").Trim()
                   .Should().Be(LocationsFiles.GetAll_json.Replace("\"", "").Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_Id(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                HttpResponseMessage response = client.GetAsync("Locations/1").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(LocationsFiles.GetById_json.Trim());
            }
        }

        [Test]
        public void Get_by_Id()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("Locations?id=Locations/1").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Locations/", "").Replace("\"", "").Trim()
                .Should().Be(LocationsFiles.GetById_json.Replace("\"", "").Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        public void Get_by_Id_not_found(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                HttpResponseMessage response = client.GetAsync("Locations/0").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Test]
        public void Get_by_Id_not_found()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.RavenApiRootUrl) })
            {
                HttpResponseMessage response = client.GetAsync("Locations?id=Locations/0").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        [TestCase(Consts.RavenApiRootUrl)]
        public void Get_by_name(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl("f");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Locations/", "").Replace("\"", "").Trim()
                .Should().Be(LocationsFiles.GetByName_json.Replace("\"", "").Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        [TestCase(Consts.RavenApiRootUrl)]
        public void Get_by_name_empty(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
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
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        [TestCase(Consts.RavenApiRootUrl)]
        public void Get_all_ordered_by_name_ascending(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 0);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Locations/", "").Replace("\"", "").Trim()
                .Should().Be(LocationsFiles.GetAllByNameAsc_json.Replace("\"", "").Trim());
            }
        }

        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        [TestCase(Consts.LinqApiRootUrl)]
        [TestCase(Consts.RavenApiRootUrl)]
        public void Get_all_ordered_by_name_descending(string root)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(root) })
            {
                var url = this.BuildDtUrl(orderColumn: 0, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Replace("Locations/", "").Replace("\"", "").Trim()
               .Should().Be(LocationsFiles.GetAllByNameDesc_json.Replace("\"", "").Trim());
            }
        }

        private string BuildDtUrl(string search = null, int? orderColumn = null, string orderDirection = null)
        {
            var url = "Locations?draw=1";
            url += "&" + WebUtility.UrlEncode("columns[0][name]") + "=Name";
            url += "&" + WebUtility.UrlEncode("columns[1][name]") + "=Id";
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
