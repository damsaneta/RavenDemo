using Demo.Model.Raven.Entities;
using Demo.RavenApi.Models;
using Demo.RavenApi.Models.DataTables;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.RavenApi.Infrastructure;
using Raven.Client.Linq;

namespace Demo.RavenApi.Controllers
{
    public class LocationsController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult Get(string id)
        {
            var result = session.Load<Location>(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new LocationDto(result));
        }

        [ResponseType(typeof(IList<LocationDto>))]
        public IHttpActionResult Get(DtRequest<LocationDto> request)
        {
            IRavenQueryable<Location> indexQuery = this.session.Query<Location,
                       Locations_ByName>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(x => x.Name.StartsWith(request.Search));
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "Name", request.OrderDirection == DtOrderDirection.DESC));
            List<LocationDto> result = indexQuery.ProjectFromIndexFieldsInto<LocationDto>()
                .ToList();
            
            return Ok(result);
        }

        public IHttpActionResult Post([FromBody]LocationDto locationDto)
        {
            var entity = new Location(locationDto);
            this.session.Store(entity);
            this.session.SaveChanges();

            return Ok(entity.Id);
        }

        public IHttpActionResult Put([FromBody]LocationDto locationDto)
        {
            Location entity = session.Load<Location>(locationDto.Id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = locationDto.Name;
            session.SaveChanges();
            return Ok(entity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                session.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
