using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.Model.EF.Dtos;
using Demo.SqlApi.Model;
using Demo.SqlApi.Model.DataTables;

namespace Demo.SqlApi.Controllers
{
    public class LocationsController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [Route("api/locations/{id}")]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult Get(short id)
        {
            LocationDto location = 
                db.Database.SqlQuery<LocationDto>("SELECT ID, Name FROM Location WHERE ID=@p0",id)
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
            var parameters = new List<object>();
            var sql = "SELECT ID, Name FROM Location ";
            if (!string.IsNullOrEmpty(request.Search))
            {
                sql += " WHERE Name LIKE @p0 ";
                parameters.Add(request.Search + "%");
            }

            request.OrderColumn = request.OrderColumn ?? "Name";
            sql += " ORDER BY " + request.OrderColumn + " " + request.OrderDirection;
            var result = db.Database.SqlQuery<LocationDto>(sql, parameters.ToArray()).ToList(); 

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
