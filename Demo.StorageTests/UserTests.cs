using Demo.Domain;
using Demo.Domain.Users;
using NUnit.Framework;
using Raven.Client.Document;
using System.Linq;
using FluentAssertions;

namespace Demo.StorageTests
{
    [TestFixture]
    public class UserTests
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

        [SetUp]
        public void Setup()
        {
            // Czyszczenie bazy
            store.DatabaseCommands.GlobalAdmin.DeleteDatabase("RavenTest", true);
            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists("RavenTest");
        }
        [Test]
        public void User_add_test()
        {
            var entity = new User("user", "Jan", "Kowalski", CryptoHelper.Hash("password"), Role.Client);

            using (var session = store.OpenSession())
            {  
                session.Store(entity);
                session.SaveChanges();
            }

            using(var session = store.OpenSession())
            {
                var entity1 = session.Load<User>(entity.Id);
                entity1.Should().NotBeNull();
                entity1.LastName.Should().Be("Kowalski");

            }
        }

        [Test]
        public void User_edit_test()
        {
            var entity1 = new User("mojUser", "Aneta", "Dams", CryptoHelper.Hash("aneta"), Role.Client);
            using (var session = store.OpenSession())
            {
                session.Store(entity1);
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var entity2 = session.Load<User>(entity1.Id);
                entity2.Should().NotBeNull();
                entity2.FirstName = "Damian";
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var entity2 = session.Load<User>(entity1.Id);
                entity2.Should().NotBeNull();
                entity2.FirstName.Should().Be("Damian");
            }
        }

        [Test]
        public void User_delete_test()
        {
            var entity1 = new User("mojUser1", "Aneta", "Dams", CryptoHelper.Hash("aneta"), Role.Client);
            using (var session = store.OpenSession())
            {
                session.Store(entity1);
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var entity2 = session.Load<User>(entity1.Id);
                entity2.Should().NotBeNull();
                session.Delete(entity2);
               // session.Delete(entity2.Id);
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var entity3 = session.Load<User>(entity1.Id);
                entity3.Should().BeNull();

            }
        }


        [Test]
        public void Get_users_by_firstName()
        {

            using (var session = store.OpenSession())
            {
                var users = session.Query<User>().Select(x => new { x.UserName, x.LastName}).ToList();
                var t = users;
            }
        }
    }
}
