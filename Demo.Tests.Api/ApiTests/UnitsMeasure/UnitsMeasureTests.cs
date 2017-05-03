using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Tests.Api.ApiTests.UnitsMeasure
{
    [TestFixture]
    class UnitsMeasureTests
    {
        [Test]
        [TestCase(Consts.SqlApiRootUrl)]
        public void Get_All(string url)
        {
            using (var client = new HttpClient {BaseAddress = new Uri(url)})
            {
                HttpResponseMessage response = client.GetAsync("UnitsMeasure").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(UnitsMeasureFiles.GetAll_json.Trim());
            }

        }

        [Test]
        public void Get_By_Id()
        {
            using (var client = new HttpClient {BaseAddress = new Uri(Consts.SqlApiRootUrl)})
            {
                HttpResponseMessage response = client.GetAsync("UnitsMeasure/car").Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(UnitsMeasureFiles.GetById_json.Trim());
            }
        }

        [Test]
        public void Get_by_id_not_found()
        {
            using (var client = new HttpClient {BaseAddress = new Uri(Consts.SqlApiRootUrl)})
            {
                HttpResponseMessage response = client.GetAsync("UnitsMeasure/x").Result;
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
                var url = this.BuildDtUrl("ca");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(UnitsMeasureFiles.GetByName_json.Trim());
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
                var url = this.BuildDtUrl(orderColumn: 0);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(UnitsMeasureFiles.GetAllOrderByIdAsc_json.Trim());
            }
        }

        [Test]
        public void Get_all_ordered_by_id_descending()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl(orderColumn: 0, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(UnitsMeasureFiles.GetAllOrderByIdDesc_json.Trim());
            }
        }

        [Test]
        public void Get_all_ordered_by_name_ascending()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl(orderColumn: 1);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(UnitsMeasureFiles.GetAllOrderByNameAsc_json.Trim());
            }
        }

        [Test]
        public void Get_all_ordered_by_name_descending()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Consts.SqlApiRootUrl) })
            {
                var url = this.BuildDtUrl(orderColumn: 1, orderDirection: "desc");
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string content = response.Content.ReadAsStringAsync().Result;
                content.Should().Be(UnitsMeasureFiles.GetAllOrderByNameDesc_json.Trim());
            }
        }

        private string BuildDtUrl(string search = null, int? orderColumn = null, string orderDirection = null)
        {
            var url = "UnitsMeasure?draw=1";
            url += "&" + WebUtility.UrlEncode("columns[0][name]") + "=UnitMeasureCode";
            url += "&" + WebUtility.UrlEncode("columns[1][name]") + "=Name";
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
