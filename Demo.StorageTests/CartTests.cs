using NUnit.Framework;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain.Carts;
using Demo.Domain.Clients;
using Demo.Domain.Products;

namespace Demo.StorageTests
{
    [TestFixture]
    public class CartTests
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
        public void Cart_add_test()
        {
            using (var session = store.OpenSession())
            {
                var client = session.Load<Client>("clients/1");
                var entity = new Cart(client);
                entity.AddToCart(session.Load<Product>("products/3"), 3);
                session.Store(entity);
                session.SaveChanges();
            }
        }

        [Test]
        public void Cart_modification_by_User_test()
        {
            using (var session = store.OpenSession())
            {
                var entity = session.Query<Cart>().SingleOrDefault(x => x.Client.Id == "clients/1");
                if(entity != null)
                {
                    var product = session.Load<Product>("products/5");
                    entity.AddToCart(product,2);
                    session.SaveChanges();
                }
              
            }
        }
    }
}
