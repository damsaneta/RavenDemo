using System;
using Demo.Model.Raven.Entities;
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
            store.Conventions.RegisterIdConvention<UnitMeasure>(((database, commands, entity) => entity.UnitMeasureCode));

            store.Initialize();

            return store;
        });

        public static IDocumentStore Store =>
            LazyStore.Value;

    }
}
