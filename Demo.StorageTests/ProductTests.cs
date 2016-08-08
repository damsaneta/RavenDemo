using Demo.Domain.Products;
using NUnit.Framework;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.StorageTests
{
    [TestFixture]
    public class ProductTests
    {
        private DocumentStore store;

        [OneTimeSetUp]
        public void SetupTests()
        {
            store = new DocumentStore() { Url = "http://localhost/RavenDB/", DefaultDatabase = "RavenTest" };
            store.Initialize();
        }

        [OneTimeTearDown]
        public void CleanUpTests()
        {
            store.Dispose();
        }

        //[SetUp]
        //public void Setup()
        //{
        //    // Czyszczenie bazy
        //    store.DatabaseCommands.GlobalAdmin.DeleteDatabase("RavenTest", true);
        //    store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists("RavenTest");
        //}
        [Test]
        public void Product_add_test()
        {
            using (var session = store.OpenSession())
            {
                for (var i = 1; i < 10; i++)
                {
                    var entity = new Product("product" + i, "opis", 100 + i);
                    session.Store(entity);
                }
                session.SaveChanges();
            }
        }

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
