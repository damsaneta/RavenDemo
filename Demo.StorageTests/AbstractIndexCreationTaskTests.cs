using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using Demo.Domain.Users;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.UniqueConstraints;
using Demo.Domain;
using FluentAssertions;
using FluentAssertions.Execution;
using Demo.Domain.Shared;

namespace Demo.StorageTests
{

    public class FullName_User : AbstractIndexCreationTask<User>
    {   
        public FullName_User()
        {
            //tworzymy index
            Map = users => from user in users
                           select new
                           {
                              FirstName =  user.FirstName,
                              LastName = user.LastName
                           };
        }
    }

    [TestFixture]
    public class AbstractIndexCreationTaskTests
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
                new FullName_User().Execute(store);
                var users = session
                    .Query<User, FullName_User>()
                    .Where(x => x.LastName == "Kowalski")
                    .ToList();
                users.Count.Should().Be(1);
                users[0].Id.Should().Be(id);
            }
        }
    }
}
