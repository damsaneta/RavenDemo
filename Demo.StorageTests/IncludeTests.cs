using NUnit.Framework;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain;
using Demo.Domain.Shared;
using Demo.Domain.Users;
using FluentAssertions;
using Demo.Domain.Products;
using Raven.Client.UniqueConstraints;

namespace Demo.StorageTests
{
    public class User1 : Entity
    {
        protected User1()
        {
        }

        public User1(string userName, string firstName, string lastName, byte[] password, Role role)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Role = role;
        }
        [UniqueConstraint]
        public string UserName { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public byte[] Password { get; private set; }

        public Role Role { get; private set; }
    }

    [TestFixture]
    public class IncludeTests
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

        [Test] // NoTrucking zapewnia, że obiekty nie sa zachowywane w pamięci więc dwa obiekty w obrebie tej samej sesji nie będą takie same
        public void Customize_NoTracking()
        {
            string id;
            using (var session = store.OpenSession())
            {
               
                var entity = new Product("product_1", "opis", 100);
                var entity1 = new Product("product_2","opis", 102);
                session.Store(entity);
                session.Store(entity1);
                id = entity.Id;
                
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var list =
                    session.Query<Product>().Customize(x => x.NoTracking()).Where(x => x.Description == "opis").ToList();
                var product = session.Load<Product>(id);
                product.Should().NotBeSameAs(list[0]);

            }


        }

        [Test]//za jednym zapytaniem możemy pobrać dane, które sa przechowywane w pamieci i z niej wyciagane
        public void Client__include()
        {
            string userId, clientId;
            var entity1 = new User("mojUser1", "Aneta", "Dams", CryptoHelper.Hash("aneta"), Role.Client);
            using (var session = store.OpenSession())
            {
                session.Store(entity1);
                userId = entity1.Id;
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var user = session.Load<User>(entity1.Id);
                user.Should().NotBeNull();
                user.LastName.Should().Be("Dams");
                var entity = new Client(user, new Address("Toruń", "Podmurna", "87-100 Toruń", "10/2", "999888777"));
                session.Store(entity);
                clientId = entity.Id;
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var client = session.Include<Client>(x => x.UserId).Load<Client>(clientId);
                var user = session.Load<User>(userId);
   
            }
        }

        [Test] //projection
        public void Projection_test()
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
            //projekcja za pomocą selecta
            using (var session = store.OpenSession())
            {
                var result =
                    session.Query<User>().Select(x => new {FirstName = x.FirstName, LastName = x.LastName}).ToList();
            }

        }
        [Test] //constraint nie działa dobrze??
        public void Constraints_Test()
        {
            using (var session = store.OpenSession())
            {
                var result = session.LoadByUniqueConstraint<User1>(x => x.UserName, "client");
            }
            using (var session = store.OpenSession())
            {
                var user = new User1("client", "Imie", "Kowalski", CryptoHelper.Hash("1234"), Role.Client);
                var checkResult = session.CheckForUniqueConstraints(user);
                if (checkResult.ConstraintsAreFree()) //czy jest dostępny
                {
                    session.Store(user);
                }
                else
                {
                    var existingUser = checkResult.DocumentForProperty(x => x.UserName);
                }        
                session.SaveChanges();
                
            }
            using (var session = store.OpenSession())
            {
                var result = session.LoadByUniqueConstraint<User1>(x => x.UserName, "client");
            } 
        }
        [Test]
        public void Bulk_insert_operation()
        {
            using (var bulkInsert = store.BulkInsert())
            {
                for (var i = 0; i < 10000; i++)
                {
                    var entity = new User("username" +i, "firstName" + i, "lastName" + i, CryptoHelper.Hash("1234"), Role.Client);
                    bulkInsert.Store(entity);
                }
            }

        }
    }
}
