using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Common.Utils;
using Demo.Domain;
using Demo.Domain.Users;
using FluentAssertions;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.UniqueConstraints;

namespace Demo.StorageTests
{
    [TestFixture]
    public class PutIndexTest
    {
        private DocumentStore store;

        [TestFixtureSetUp]
        public void SetupTests()
        {
            store = new DocumentStore() { Url = "http://localhost/RavenDB/", DefaultDatabase = "RavenTest" };
            store.Initialize();
            store.RegisterListener(new UniqueConstraintsStoreListener());
        }

        [TestFixtureTearDown]
        public void CleanUpTests()
        {
            store.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            store.DatabaseCommands.GlobalAdmin.DeleteDatabase("RavenTest", true);
            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists("RavenTest");
        }
        [Test]
        public void InitUsers()
        {
            string id;
            using (var session = store.OpenSession())
            {
                var pass = CryptoHelper.Hash("1234");
                var client = new User("client", "Jan", "Kowalski", pass, Role.Client);
                var admin = new User("admin", "Aneta", "Dams", pass, Role.Administrator);
                session.Store(client);
                session.Store(admin);
                id = client.Id;
                for (var i = 0; i < 10; i++)
                {
                    var user = new User("client_" + i, "Imie_" + i, "Kowalski_" + i, pass, Role.Client);
                    session.Store(user);
                }
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                store.DatabaseCommands.PutIndex("FullName/User", new IndexDefinitionBuilder<User>() //PutIndex, ale lepsza wersja to abstractIndexCreationTask
                {
                    Map = users => from user in users
                        select new
                        {
                            user.FirstName,
                            user.LastName
                        }
                });
                var result = session
                    .Query<User, FullName_User>()
                    .Where(x => x.LastName == "Kowalski")
                    .ToList();
                result.Count.Should().Be(1);
                result[0].Id.Should().Be(id);
            }
        }
    }
}
