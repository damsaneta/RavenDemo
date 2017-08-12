using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class UnitMeasures_ByNameAndUnitMeasureCodeSortByNameUnitMeasureCode : AbstractIndexCreationTask<UnitMeasure>
    {
        public UnitMeasures_ByNameAndUnitMeasureCodeSortByNameUnitMeasureCode()
        {
            Map = unitMeasures => from unitMeasure in unitMeasures
                select new
                {
                    UnitMeasureCode = unitMeasure.UnitMeasureCode,
                    Name = unitMeasure.Name
                };
        }

    }
}