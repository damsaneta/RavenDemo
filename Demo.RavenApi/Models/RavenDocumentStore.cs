using System;
using System.Linq;
using Demo.Model.Raven.Entities;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Demo.RavenApi.Models
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

    public class ProductCategories_ByIdSortById : AbstractIndexCreationTask<ProductCategory>
    {
        public ProductCategories_ByIdSortById()
        {
            Map = productCategories => from productCategory in productCategories
                select new
                {
                    Name = productCategory.Name
                };
        }
    }

    public class ProductCategories_ByNameSortByIdName : AbstractIndexCreationTask<ProductCategory>
    {
        public ProductCategories_ByNameSortByIdName()
        {
            Map = productCategories => from productCategory in productCategories
                select new
                {
                    __document_id = productCategory.Id
                };
        }
    }

    public class ProductSubcategories_ByNameAndProductCategoryNameSortByNameProductCategoryName :
        AbstractIndexCreationTask<ProductSubcategory>
    {
        public ProductSubcategories_ByNameAndProductCategoryNameSortByNameProductCategoryName()
        {
            Map = productSubcategories => from productSubcategory in productSubcategories
                select new
                {
                    //ProductCategoryName = productSubcategory.ProductCategoryName,
                    Name = productSubcategory.Name
                };
        }
    }

    public class UnitMeasures_ByNameAndUnitMeasureCodeSortByNameUnitMeasureCode : AbstractIndexCreationTask<UnitMeasure>
    {
        public UnitMeasures_ByNameAndUnitMeasureCodeSortByNameUnitMeasureCode()
        {
            Map = unitMeasures => from unitMeasure in unitMeasures
                select new
                {
                    UnitMeasureCode = unitMeasure.UnitMeasureCode,
                    Name = unitMeasure.Name
                };
        }

    }

    public class Products_ByColorAndNameAndProductNumberAndProductSubcategoryNameSortByNameProductSubcategoryName :
        AbstractIndexCreationTask<Product>
    {
        public Products_ByColorAndNameAndProductNumberAndProductSubcategoryNameSortByNameProductSubcategoryName()
        {
            Map = products => from product in products
                select new
                {
                    //ProductSubcategoryName = product.ProductSubcategoryName,
                    ProductNumber = product.ProductNumber,
                    Color = product.Color,
                    Name = product.Name
                };
        }
    }
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

            return store;
        });

        public static IDocumentStore Store =>
            LazyStore.Value;

    }
}
