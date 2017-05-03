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
    public class UnitsMeasureController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [Route("api/unitsMeasure/{id}")]
        [ResponseType(typeof(UnitMeasureDto))]
        public IHttpActionResult Get(string id)
        {
            throw new NotImplementedException();
            UnitMeasureDto unitMeasure = db.Database.SqlQuery<UnitMeasureDto>(@"SELECT UnitMeasureCode,Name FROM UnitMeasure 
                WHERE UnitMeasureCode=@p0", id).SingleOrDefault();
            if(unitMeasure == null)
            {
                return NotFound();
            }

            return Ok(unitMeasure);
        }

        [ResponseType(typeof(IList<UnitMeasureDto>))]
        public IHttpActionResult Get(DtRequest<UnitMeasureDto> request)
        {
            throw new NotImplementedException();
            var parameters = new List<object>();
            var sql = "SELECT UnitMeasureCode, Name FROM UnitMeasure ";
            if (!string.IsNullOrEmpty(request.Search))
            {
                sql += " WHERE Name LIKE @p0 ";
                parameters.Add(request.Search);
            }

            request.OrderColumn = request.OrderColumn ?? "Name";
            sql += " ORDER BY " + request.OrderColumn + " " + request.OrderDirection;
            var result = db.Database.SqlQuery<UnitMeasureDto>(sql, parameters.ToArray()).ToList();

            return Ok(result);
        }
    }
}
