using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.Raven
{
    public class RavenDocumentStore
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
        new Lazy<IDocumentStore>(() =>
        {
            var store = new DocumentStore
            {
                Url = "http://localhost/RavenDB",
                DefaultDatabase = "AWL"
            };

            return store.Initialize();
        });

        public static IDocumentStore Store =>
            LazyStore.Value;

    }
}
