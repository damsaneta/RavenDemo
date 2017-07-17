using System;
using Raven.Client;
using Raven.Client.Document;

namespace Demo.RavenApi.Models
{
    public class RavenDocumentStore
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
        new Lazy<IDocumentStore>(() =>
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "Server"
            };

            return store.Initialize();
        });

        public static IDocumentStore Store =>
            LazyStore.Value;

    }
}
