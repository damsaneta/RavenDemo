using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Common.Utils;
using Demo.Domain;
using Demo.Domain.Users;
using NUnit.Framework;
using Demo.Domain.Products;
using Raven.Client.Document;
using Demo.Domain.Shared;
using FluentAssertions;
using Raven.Client;

namespace Demo.StorageTests
{
    [TestFixture]
    public class InitDatabase
    {
        private DocumentStore store;

        [TestFixtureSetUp]
        public void SetupTests()
        {
            store = new DocumentStore() { Url = "http://localhost/RavenDB/", DefaultDatabase = "RavenDemo" };
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
            store.DatabaseCommands.GlobalAdmin.DeleteDatabase("RavenDemo", true);
            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists("RavenDemo");
        }

        [Test]
        [Explicit]
        public void Init()
        {
            this.InitUsers();
            this.InitProducts();
            InitClients();
        }

        private void InitUsers()
        {
            using (var session = store.OpenSession())
            {
                var pass = CryptoHelper.Hash("1234");
                var client = new User("client", "Jan", "Kowalski", pass, Role.Client);
                var admin = new User("admin", "Aneta", "Dams", pass, Role.Administrator);
                session.Store(client);
                session.Store(admin);
                for (var i = 0; i < 10; i++)
                {
                    var user = new User("client_" + i, "Imie_" + i, "Kowalski_" + i, pass, Role.Client);
                    session.Store(user);
                }

                session.SaveChanges();
            }
        }

        private void InitClients()
        {
            using (var session = store.OpenSession())
            {
                var users = session.Query<User>().Where(x => x.Role == Role.Client)
                    .OrderBy(x => x.Id)
                    .ToList();
                User user;
                Client client;
                for (int i = 0; i < users.Count; i++)
                {
                    user = users[i];
                    client = new Client(user, new Address("City_" + i, "Street_" + i, "87-10" + i, i.ToString(), "000-000-000"));
                    session.Store(client);
                }

                user = session.Query<User>().Single(x => x.UserName == "client");
                client = new Client(user, new Address("Toruń", "Chodkiewicza", "87-100", "10/2", "222-111-333"));
                session.Store(client);
                session.SaveChanges();
            }
        }

        [Test]
        public void InitClients2()
        {
            string id;
            using (var session = store.OpenSession())
            {
                var pass = CryptoHelper.Hash("1234");
                var user = new User("client", "Jan", "Kowalski", pass, Role.Client);
                session.Store(user);
                var client1 = new Client(user, new Address("Toruń", "Chodkiewicza", "87-100", "10/2", "222-111-333"));
                var client2 = new Client(user, new Address("Toruń", "Chodkiewicza", "87-100", "10/2", "222-111-333"));
                session.Store(client1);
                session.Store(client2);
                id = user.Id;
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var clients = session.Query<Client>()
                    .Include(x => x.UserId)
                    .ToList();
                var user = session.Load<User>(id);
                //clients[0].User.Should().Be(clients[1].User);
            }
        }

        private void InitProducts()
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
    }
}
