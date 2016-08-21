using Demo.Domain.Shared;
using Raven.Client.Document;

namespace Demo.Storage.Repositories.Impl
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        public DocumentStore DocumentStore { get; private set; }

        protected Repository(DocumentStore documentStore)
        {
            DocumentStore = documentStore;
        }
    }
}
