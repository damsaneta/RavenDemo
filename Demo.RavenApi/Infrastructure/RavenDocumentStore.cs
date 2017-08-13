using System;
using Demo.Model.Raven.Entities;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Infrastructure
{
    public static class RavenDocumentStore
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
        new Lazy<IDocumentStore>(() =>
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "Server"
            };
            store.Conventions.RegisterIdConvention<UnitMeasure>(((database, commands, entity) => entity.UnitMeasureCode));
           

            store.Initialize();
            IndexCreation.CreateIndexes(typeof(RavenDocumentStore).Assembly, store);
            return store;
        });

        public static IDocumentStore Store => LazyStore.Value;
    }
}
