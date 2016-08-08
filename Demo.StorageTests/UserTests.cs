using Demo.Domain;
using Demo.Domain.Users;
using NUnit.Framework;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Raven.Client;
using Raven.Client.Connection.Profiling;
using Raven.Client.Listeners;

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
            store.InitializeProfiling();
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
        public void User_add_test()
        {
            using (var session = store.OpenSession())
            {
                for (var i = 1; i < 10; i++)
                {
                    var password = CryptoHelper.Hash("aaaaaa" + i);
                    var entity = new User("user" + i, "firstName" + i, "lastName" + i, password, Role.Client);
                    session.Store(entity);
                }
                var entity1 = new User("mojUser", "Aneta", "Dams", CryptoHelper.Hash("aneta"), Role.Client);
                session.Store(entity1);
                session.SaveChanges();
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
            using (var session = store.OpenSession())
            {
                // session.Delete(session.Load<User>("users/4"));
                session.Delete("users/2");
                session.SaveChanges();
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
