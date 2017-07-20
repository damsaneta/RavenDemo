namespace Demo.Model.Raven.Entities
{
    public class LocationDto
    {
        public LocationDto()
        {
        }

        public LocationDto(Location entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}