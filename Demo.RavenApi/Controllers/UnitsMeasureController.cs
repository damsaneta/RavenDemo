using System;
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
    public class UnitsMeasureController : ApiController
    {
        private readonly IDocumentSession session = RavenDocumentStore.Store.OpenSession();

        [ResponseType(typeof(UnitMeasureDto))]
        public IHttpActionResult Get(string id)
        {
            var result = session.Load<UnitMeasure>(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new UnitMeasureDto(result));
        }

        [ResponseType(typeof(IList<UnitMeasureDto>))]
        public IHttpActionResult Get(DtRequest<UnitMeasureDto> request)
        {
            IRavenQueryable<UnitMeasure> indexQuery = this.session.Query<UnitMeasure,
                       UnitMeasures_ByNameAndUnitMeasureCode>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                indexQuery = indexQuery.Where(x => x.Name.StartsWith(request.Search) || x.UnitMeasureCode.StartsWith(request.Search));
            }
            else if (request.SearchByColumnValues.Any())
            {
                foreach (var searchByColumnValue in request.SearchByColumnValues)
                {
                    var columnName = searchByColumnValue.Key;
                    var searchValue = searchByColumnValue.Value;
                    switch (columnName)
                    {
                        case "Name":
                            indexQuery = indexQuery.Where(x => x.Name.StartsWith(searchValue));
                            break;
                        case "UnitMeasureCode":
                            indexQuery = indexQuery.Where(x => x.UnitMeasureCode.StartsWith(searchValue));
                            break;
                        default: throw new ArgumentException("Nieznana kolumna", columnName);
                    }
                }
            }

            indexQuery = indexQuery.Customize(x => x.AddOrder(request.OrderColumn ?? "Name", request.OrderDirection == DtOrderDirection.DESC));
            List<UnitMeasureDto> result = indexQuery.ProjectFromIndexFieldsInto<UnitMeasureDto>()
                .ToList();

            return Ok(result);
        }

        public IHttpActionResult Post([FromBody]UnitMeasureDto unitMeasureDto)
        {
            var entity = new UnitMeasure(unitMeasureDto);
            this.session.Store(entity, "UnitsMeasures/" + unitMeasureDto.UnitMeasureCode);
            this.session.SaveChanges();

            return Ok(entity.UnitMeasureCode);
        }

        public IHttpActionResult Put([FromBody]UnitMeasureDto unitMeasureDto)
        {
            UnitMeasure entity = session.Load<UnitMeasure>("UnitsMeasures/" + unitMeasureDto.UnitMeasureCode);
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
