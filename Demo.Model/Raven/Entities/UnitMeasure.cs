namespace Demo.Model.Raven.Entities
{
    public class UnitMeasure
    {
        public UnitMeasure()
        {

        }

        public UnitMeasure(UnitMeasureDto dto)
        {
            this.UnitMeasureCode = dto.UnitMeasureCode;
            this.Name = dto.Name;
        }

        public string UnitMeasureCode { get; set; }

        public string Name { get; set; }
    }
}