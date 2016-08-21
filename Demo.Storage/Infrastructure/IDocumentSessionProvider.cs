using Raven.Client;

namespace Demo.Storage.Infrastructure
{
    public interface IDocumentSessionProvider
    {
        IDocumentSession Create();
    }
}
