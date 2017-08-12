using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Locations_ByNameSortByName : AbstractIndexCreationTask<Location>
    {
        public Locations_ByNameSortByName()
        {
            Map = locations => from location in locations
                select new
                {
                    Name = location.Name
                };
        }
    }
}