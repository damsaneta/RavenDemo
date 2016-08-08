using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain;
using Demo.Domain.Users;
using NUnit.Framework;
using Demo.Domain.Products;

namespace Demo.StorageTests
{
    [TestFixture]
    public class InitDatabase
    {
        private IList<User> users;
        private IList<Product> products;

        [Test]
        [Explicit]
        public void Init()
        {
            this.InitUsers();
            this.InitProducts();
        }

        private void InitUsers()
        {
            for (var i = 1; i < 10; i++)
            {
                var password = CryptoHelper.Hash("aaaaaa" + i);
                var entity = new User("user" + i, "firstName" + i, "lastName" + i, password, Role.Client);
                users.Add(entity);
            }
            var entity1 = new User("mojUser", "Aneta", "Dams", CryptoHelper.Hash("aneta"), Role.Client);
            users.Add(entity1);
        }

        private void InitProducts()
        {
            for (var i = 1; i < 10; i++)
            {
                var entity = new Product("product" + i, "opis", 100 + i);
                products.Add(entity);
            }

        }
    }
}
