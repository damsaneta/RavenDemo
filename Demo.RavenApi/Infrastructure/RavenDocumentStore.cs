using System;
using Demo.Model.Raven.Entities;
using Raven.Client;
using Raven.Client.Document;

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

            //store.getDatabaseCommands().deleteIndex("Orders/Totals");
         
            store.Initialize();

            new UnitMeasures_ByNameAndUnitMeasureCodeSortByNameUnitMeasureCode().Execute(store);

            new ProductCategories_ByIdSortById().Execute(store);

            new ProductSubcategories_ByNameAndProductCategoryNameSortByNameProductCategoryName().Execute(store);

            new Locations_ByNameSortByName().Execute(store);

            new Products_ByColorAndNameAndProductNumberAndProductSubcategoryNameSortByNameProductSubcategoryName().Execute(store);
            new Subcategories_ByCategoryName().Execute(store);
            return store;
        });

        public static IDocumentStore Store =>
            LazyStore.Value;

    }
}
