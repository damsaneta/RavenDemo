using Demo.Domain.Products;
using NUnit.Framework;
using Raven.Client.Document;
using FluentAssertions;

namespace Demo.StorageTests
{
    [TestFixture]
    public class ProductTests
    {
        private DocumentStore store;

        [TestFixtureSetUp]
        public void SetupTests()
        {
            store = new DocumentStore() { Url = "http://localhost/RavenDB/", DefaultDatabase = "RavenTest" };
            store.Initialize();
        }

        [TestFixtureTearDown]
        public void CleanUpTests()
        {
            store.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            // Czyszczenie bazy
            store.DatabaseCommands.GlobalAdmin.DeleteDatabase("RavenTest", true);
            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists("RavenTest");
        }
        //[Test]
        //public void Product_add_test()
        //{
            
        //    var entity = new Product("product", "opis", 100);
        //    using (var session = store.OpenSession())
        //    {  
        //        session.Store(entity);
        //        session.SaveChanges();
        //    }

        //    using(var session = store.OpenSession())
        //    {
        //        var entity1 = session.Load<Product>(entity.Id);
        //        entity1.Should().NotBeNull();
        //        entity1.Name.Should().Be("product");

        //    }
        
        //}

        [Test]
        public void Product_delete_test()
        {
            using (var session = store.OpenSession())
            {
                session.Delete("products/6");
                session.SaveChanges();
            }
        }

    }
}
