using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain;
using Demo.Domain.Users;
using Demo.Storage.Repositories.Impl;
using FluentAssertions;
using NUnit.Framework;
using Raven.Client.Document;
using Demo.Domain.Orders;
using Demo.Domain.Products;
using Demo.Domain.Shared;
using Raven.Client;

namespace Demo.StorageTests
{
    [TestFixture]
    public class RepositoryTests
    {
        private DocumentStore store;
        private UserRepository userRepository;
        private CartRepository cartRepository;

        [TestFixtureSetUp]
        public void SetupTests()
        {
            store = new DocumentStore() { Url = "http://localhost/RavenDB/", DefaultDatabase = "RavenTest" };
            store.Initialize();
            userRepository = new UserRepository(store);
            cartRepository = new CartRepository(store);
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
        public void GetByUserNameTest()
        {
            var entity = new User("testUser", "Jan", "Kowalski", CryptoHelper.Hash("password"), Role.Client);
            using (var session = store.OpenSession())
            {
                for(var i = 1; i < 10; i++)
                {
                    var entity1 = new User("user_" + i, "Jan_" + i, "Kowalski_" + i, CryptoHelper.Hash("password"), Role.Client);
                    session.Store(entity1);
                }
                session.Store(entity);
                session.SaveChanges();
            }
            var entity2 = userRepository.GetByUserName(entity.UserName);
            entity2.Should().NotBeNull();
            entity2.UserName.Should().Be("testUser");
        }

        [Test]
        public void GetCartForUser()
        {
            string  cartId, userId; 
            using (var session = store.OpenSession())
            {
                var userEntity3 = new User("user", "user", "user", CryptoHelper.Hash("1234"), Role.Client);
                session.Store(userEntity3);
                userId = userEntity3.Id;
                var clientEntity3 = new Client(userEntity3,
                    new Address("Toruń", "Podmurna", "87-100", "10/2", "000-000-000"));
                session.Store(clientEntity3);
                for (var i = 0; i < 10; i++)
                {
                    var userEntity = new User("user_" + i, "user"+i, "user"+i, CryptoHelper.Hash("1234"), Role.Client);
                    session.Store(userEntity);
                    var clientEntity = new Client(userEntity,
                        new Address("Toruń", "Podmurna", "87-100", "10/2", "000-000-000"));
                    session.Store(clientEntity);

                    for (var j = 1; j < 10; j++)
                    {
                        var entity = new Product("product" + j, "opis", 100 + j);
                        session.Store(entity);
                    }

                    var cart = new Cart(clientEntity);
                    cart.AddToCart(session.Load<Product>("products/3"), 3);
                    cart.AddToCart(session.Load<Product>("products/2"), 2);
                    session.Store(cart);
                }

                
 

                for (var j = 1; j < 10; j++)
                {
                    var entity = new Product("product" + j, "opis", 100 + j);
                    session.Store(entity);
                }

                var cart3 = new Cart(clientEntity3);
                cart3.AddToCart(session.Load<Product>("products/3"), 3);
                cart3.AddToCart(session.Load<Product>("products/2"), 2);
                session.Store(cart3);
                cartId = cart3.Id;
                session.SaveChanges();
            }
            //using (var session = store.OpenSession())
            //{
                
            //    var o = session.Load<Client>("client/1");
            //    var k = session.Load<User>("user/1");
            //   // var result = session.Include<Cart>(t=> t.ClientId.).Load<Cart>();
            //}
         
            var result = cartRepository.GetCartForUser(userId);
            result.Id.Should().Be(cartId);
            result.Should().NotBeNull();
                //t.AddToCart(session.Load<Product>("products/3"), 1);
            
        
        }
    }
}
