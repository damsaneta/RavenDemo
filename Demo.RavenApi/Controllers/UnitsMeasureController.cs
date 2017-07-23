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
    public class UnitsMeasureController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(UnitMeasureDto))]
        public IHttpActionResult Get(string id)
        {
            var result = session.Load<UnitMeasure>("UnitsMeasure/" + id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new UnitMeasureDto(result));
        }

        [ResponseType(typeof(IList<UnitMeasureDto>))]
        public IHttpActionResult Get(DtRequest<UnitMeasureDto> request)
        {
            IQueryable<UnitMeasure> query = this.session.Query<UnitMeasure>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.StartsWith(request.Search));
            }

            var queryDto = query.Select(x => new UnitMeasureDto
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

        public IHttpActionResult Post([FromBody]UnitMeasureDto unitMeasureDto)
        {
            var entity = new UnitMeasure(unitMeasureDto);
            this.session.Store(entity);
            this.session.SaveChanges();

            return Ok(entity.UnitMeasureCode);
        }

        public IHttpActionResult Put([FromBody]UnitMeasureDto unitMeasureDto)
        {
            UnitMeasure entity = session.Load<UnitMeasure>(unitMeasureDto.UnitMeasureCode);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = unitMeasureDto.Name;
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
