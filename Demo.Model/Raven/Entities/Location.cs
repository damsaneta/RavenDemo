namespace Demo.Model.Raven.Entities
{
    public class Location
    {
        public Location()
        {
        }

        public Location(LocationDto dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}