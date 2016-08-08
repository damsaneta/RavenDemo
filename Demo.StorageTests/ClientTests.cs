using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Demo.Domain;
using Demo.Domain.Clients;
using Demo.Domain.Products;
using Demo.Domain.Shared;
using Demo.Domain.Users;
using FluentAssertions;
using NUnit.Framework;
using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Demo.StorageTests
{
    [TestFixture]
    public class ClientTests
    {
        private DocumentStore store;
        
        [OneTimeSetUp]
        public void SetupTests()
        {
            store = new DocumentStore() {Url = "http://localhost/RavenDB/", DefaultDatabase = "RavenTest"};
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
        public void Client_add_test()
        {
            using (var session = store.OpenSession())
            {
                var user = session.Load<User>("users/10");
                var entity = new Client("Client T1", user, new Address("Toruń", "Podmurna", "87-100 Toruń", "10/2", "999888777"));
                session.Store(entity);
                session.SaveChanges();
                var id = entity.Id;
                id.Should().NotBeNullOrEmpty();
            }
        }

     

        //[Test]
        //public void Client_add_and_load_in_the_same_session_test()
        //{
        //    using (var session = store.OpenSession())
        //    {
        //        var entity = new Client("Client T2");
        //        session.Store(entity);
        //        session.SaveChanges();
        //        string id = entity.Id;
        //        var entity2 = session.Load<Client>(id);
        //        entity2.Should().NotBeNull();
        //        entity2.Should().BeSameAs(entity);
        //    }
        //}

        //[Test]
        //public void Client_add_and_load_in_different_sessions_test()
        //{
        //    Client entity = new Client("Client T3");
        //    string id;
        //    using (var session = store.OpenSession())
        //    {
        //        session.Store(entity);
        //        session.SaveChanges();
        //        id = entity.Id;
        //    }

        //    using (var session = store.OpenSession())
        //    {
        //        var entity2 = session.Load<Client>(id);
        //        entity2.Should().NotBeNull();
        //        entity2.Should().NotBeSameAs(entity);
        //        var entity3 = session.Load<Client>(id);
        //        entity3.Should().BeSameAs(entity3);
        //    }
       // }
    }
}
