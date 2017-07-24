using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.LinqApi.Model;
using Demo.LinqApi.Model.DataTables;
using Demo.Model.EF.Dtos;
using Demo.Model.EF.Entities;

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
                .Where(x => x.Id == id)
                .Select(x => new LocationDto
                {
                    Id = x.Id,
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
