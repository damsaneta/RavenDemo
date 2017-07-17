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
    public class UnitsMeasureController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [Route("api/unitsMeasure/{id}")]
        [ResponseType(typeof(UnitMeasureDto))]
        public IHttpActionResult Get(string id)
        {
    
            var unitMeasure = db.Set<UnitMeasure>()
                .Where(x => x.UnitMeasureCode == id)
                .Select(x => new UnitMeasureDto
                {
                    UnitMeasureCode = x.UnitMeasureCode,
                    Name = x.Name
                })
                .SingleOrDefault();

            if(unitMeasure == null)
            {
                return NotFound();
            }

            return Ok(unitMeasure);
        }

        [ResponseType(typeof(IList<UnitMeasureDto>))]
        public IHttpActionResult Get(DtRequest<UnitMeasureDto> request)
        {
            //throw new NotImplementedException();
            IQueryable<UnitMeasure> query = db.Set<UnitMeasure>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.StartsWith(request.Search));
            }
            IQueryable<UnitMeasureDto> queryDto = query.Select(x => new UnitMeasureDto
            {
                UnitMeasureCode = x.UnitMeasureCode,
                Name = x.Name
            });

            switch (request.OrderColumn)
            {
                case "UnitMeasureCode":
                    queryDto = request.OrderDirection == DtOrderDirection.ASC
                        ? queryDto.OrderBy(x => x.UnitMeasureCode)
                        : queryDto.OrderByDescending(x => x.UnitMeasureCode);
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
