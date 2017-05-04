using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.LinqApi.Model.DataTables;
using Demo.Model.Dtos;
using Demo.Model.Entities;

namespace Demo.LinqApi.Controllers
{
    public class LocationsController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [Route("api/locations/{id}")]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult Get(short id)
        {
            var location = db.Set<Location>()
                .Where(x => x.ID == id)
                .Select(x => new LocationDto
                {
                    ID = x.ID,
                    Name = x.Name
                })
                .SingleOrDefault();

            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [ResponseType(typeof(IList<LocationDto>))]
        public IHttpActionResult Get(DtRequest<LocationDto> request)
        {
            IQueryable<Location> query = db.Set<Location>();
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.StartsWith(request.Search));
            }

            IQueryable<LocationDto> queryDto = query.Select(x => new LocationDto
            {
                ID = x.ID,
                Name = x.Name
            });

            switch (request.OrderColumn)
            {
                case "ID":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.ID)
                        : queryDto.OrderByDescending(x => x.ID); 
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
