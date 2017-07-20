namespace Demo.Model.Raven.Entities
{
    public class UnitMeasureDto
    {
        public UnitMeasureDto()
        {

        }

        public UnitMeasureDto(UnitMeasure entity)
        {
            this.UnitMeasureCode = entity.UnitMeasureCode;
            this.Name = entity.Name;
        }

        public string UnitMeasureCode { get; set; }

        public string Name { get; set; }
    }
}