using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Shared;
using Demo.Storage.Infrastructure;
using Raven.Client;

namespace Demo.Storage.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly IDocumentSessionProvider provider;

        protected Repository(IDocumentSessionProvider provider)
        {
            this.provider = provider;
        }

        public IDocumentSession DocumentSession { get { return this.provider.Create(); } }

        public void SaveChanges()
        {
            this.DocumentSession.SaveChanges();
        }

        public void Add(T entity)
        {
            this.DocumentSession.Store(entity);
            this.DocumentSession.SaveChanges();
        }

        public void Add(params T[] entities)
        {
            foreach (var entity in entities)
            {
                this.DocumentSession.Store(entity);
            }

            this.DocumentSession.SaveChanges();
        }

        public void Delete(T entity)
        {
            this.DocumentSession.Delete(entity);
            this.DocumentSession.SaveChanges();
        }

        public void Delete(string id)
        {
            var entity = this.DocumentSession.Load<T>();
            this.Delete(entity);
        }

        public void Delete(params T[] entities)
        {
            foreach (var entity in entities)
            {
                this.DocumentSession.Delete(entity);
            }

            this.DocumentSession.SaveChanges();
        }

        public void Delete(params string[] ids)
        {
            foreach (var id in ids)
            {
                var entity = this.DocumentSession.Load<T>(id);
                this.DocumentSession.Delete(entity);
            }

            this.DocumentSession.SaveChanges();
        }

        public T Load(string id)
        {
            return this.DocumentSession.Load<T>(id);
        }

        public IList<T> LoadAll()
        {
            return this.DocumentSession.Query<T>().ToList();
        }
    }
}
