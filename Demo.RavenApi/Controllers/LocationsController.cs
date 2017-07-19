using Demo.Model.Raven.Entities;
using Demo.RavenApi.Models;
using Demo.RavenApi.Models.DataTables;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Demo.RavenApi.Controllers
{
    public class LocationsController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        public IHttpActionResult Get(string id)
        {
            var result = session.Load<Location>("locations/"+id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [ResponseType(typeof(IList<Location>))]
        public IHttpActionResult Get(DtRequest<Location> request)
        {
            var result = session.Query<Location>().ToList();
            return Ok(result);
        }

        public IHttpActionResult Post([FromBody]Location location)
        {
            session.Store(location);
            session.SaveChanges();
            return Ok(location.Id);
        }

        public IHttpActionResult Put([FromBody]Location location)
        {
            Location currentLocation = session.Load<Location>(location.Id);
            if (currentLocation == null)
            {
                return NotFound();
            }

            currentLocation.Name = location.Name;
            session.SaveChanges();
            return Ok(currentLocation);
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
