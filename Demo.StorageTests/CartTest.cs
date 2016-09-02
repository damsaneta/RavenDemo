using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Common.Utils;
using Demo.Domain;
using Demo.Domain.Orders;
using Demo.Domain.Products;
using Demo.Domain.Shared;
using NUnit.Framework;
using Raven.Client.Document;
using Demo.Domain.Users;
using FluentAssertions;

namespace Demo.StorageTests
{
    [TestFixture]
    public class CartTest
    {
        private DocumentStore store;

        [TestFixtureSetUp]
        public void SetupTests()
        {
            store = new DocumentStore() {Url = "http://localhost/RavenDB/", DefaultDatabase = "RavenTest"};
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
        [Test]
        public void Cart_add_test()
        {
            string userId, clientId;
            var userEntity = new User("user", "user", "user", CryptoHelper.Hash("1234"), Role.Client);
            //dodaje usera
            using (var session = store.OpenSession())
            {
                session.Store(userEntity);
                userId = userEntity.Id;
                session.SaveChanges();

            }
            //dodaje clienta
            using (var session = store.OpenSession())
            {
                var user = session.Load<User>(userId);
                user.Should().NotBeNull();
                user.UserName.Should().Be("user");

                var clientEntity = new Client(user, new Address("Toruń", "Podmurna", "87-100", "10/2", "000-000-000"));
                session.Store(clientEntity);
                clientId = clientEntity.Id;
                session.SaveChanges();
            }
            using (var session = store.OpenSession())
            {
                for (var i = 1; i < 10; i++)
                {
                    var entity = new Product("product" + i, "opis", 100 + i);
                    session.Store(entity);
                }
                session.SaveChanges();
            }

            string cartId;
            using (var session = store.OpenSession())
            {
                var client = session.Load<Client>(clientId);
                client.Should().NotBeNull();
                client.UserId.Should().Be(userId);

                var cart = new Cart(client);
                cart.AddToCart(session.Load<Product>("products/3"), 3);
                cart.AddToCart(session.Load<Product>("products/2"), 2);
                session.Store(cart);
                cartId = cart.Id;
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var cart = session.Load<Cart>(cartId);
                cart.Should().NotBeNull();
                cart.AddToCart(session.Load<Product>("products/3"), 1);
            }
        }


    }
}