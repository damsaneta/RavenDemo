using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class Locations_ByName : AbstractIndexCreationTask<Location>
    {
        public Locations_ByName()
        {
            Map = locations => from location in locations
                select new
                {
                    Name = location.Name
                };
        }
    }
}