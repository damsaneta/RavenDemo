using Demo.Model.Raven.Entities;
using Demo.RavenApi.Models;
using Demo.RavenApi.Models.DataTables;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.RavenApi.Infrastructure;

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
            IQueryable<Location> query = this.session.Query<Location>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.StartsWith(request.Search));
            }

            var queryDto = query.Select(x => new LocationDto
            {
                Id = x.Id,
                Name = x.Name
            });

            switch (request.OrderColumn)
            {
                case "Id":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Id)
                        : queryDto.OrderByDescending(x => x.Id);
                    break;
                default:
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.Name)
                        : queryDto.OrderByDescending(x => x.Name);
                    break;
            }
            var result = queryDto.ToList();
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
