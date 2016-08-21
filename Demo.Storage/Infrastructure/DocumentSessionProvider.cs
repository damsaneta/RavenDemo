using System;
using Raven.Client;

namespace Demo.Storage.Infrastructure
{
    public class DocumentSessionProvider : IDocumentSessionProvider
    {
        private readonly Func<IDocumentSession> func;

        public DocumentSessionProvider(Func<IDocumentSession> func)
        {
            this.func = func;
        }

        public IDocumentSession Create()
        {
            return func();
        }
    }
}
