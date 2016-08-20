using System.Linq;
using Demo.Domain;
using Demo.Domain.Shared;
using Demo.Domain.Users;
using FluentAssertions;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Linq;


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

        [SetUp]
        public void Setup()
        {
            // Czyszczenie bazy
            store.DatabaseCommands.GlobalAdmin.DeleteDatabase("RavenTest", true);
            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists("RavenTest");
        }

        [Test]
        public void Client_add_test()
        {
            var entity1 = new User("mojUser1", "Aneta", "Dams", CryptoHelper.Hash("aneta"), Role.Client);
            using (var session = store.OpenSession())
            {
                session.Store(entity1);
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var user = session.Load<User>(entity1.Id);
                user.Should().NotBeNull();
                user.LastName.Should().Be("Dams");

                var entity = new Client(user, new Address("Toruń", "Podmurna", "87-100 Toruń", "10/2", "999888777"));
                session.Store(entity);
                session.SaveChanges();
                entity.Id.Should().NotBeNullOrEmpty();
            }
        }  

        [Test]
        public void Client_simple_querry()
        {
            using (var session = store.OpenSession())
            {
                for(int i = 1; i <20; i++)
                {
                    var entity1 = new User("user"+i, "test", "testLastName"+i, CryptoHelper.Hash("aneta"), Role.Client);
                    session.Store(entity1);
                }
               
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var result = session.Query<User>().Where(x => x.FirstName == "test").ToList();
                result.Count.Should().Be(19);
            }

            using (var session = store.OpenSession())
            {
                var result = from user in session.Query<User>() where user.FirstName == "test" select user;
                result.ToList().Count.Should().Be(19);
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
