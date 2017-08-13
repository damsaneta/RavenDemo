using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public class ProductSubcategories_ByName : AbstractIndexCreationTask<ProductSubcategory>
    {
        public ProductSubcategories_ByName()
        {
            Map = productSubcategories => from productSubcategory in productSubcategories
                select new
                {
                    Name = productSubcategory.Name
                };
            this.Index(x => x.Name, FieldIndexing.Analyzed);
        }
    }
}