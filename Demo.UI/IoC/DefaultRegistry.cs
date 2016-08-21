using Demo.Storage.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;

namespace Demo.UI.IoC 
{
    public class DefaultRegistry : Registry 
    {
        public DefaultRegistry()
        {
            For<DocumentStore>()
                .Use(() => CreateDocumentStore())
                .Singleton();
            For<IDocumentSession>().Use(c => c.GetInstance<DocumentStore>().OpenSession()).Transient();
            For<IDocumentSessionProvider>()
                .Use(new DocumentSessionProvider(() => ServiceLocator.Current.GetInstance<IDocumentSession>()))
                .Singleton();
            
            Scan(
                scan => {
                    scan.TheCallingAssembly();
					scan.With(new ControllerConvention());
                });
            Policies.FillAllPropertiesOfType<IDocumentSessionProvider>();
        }

        private static DocumentStore CreateDocumentStore()
        {
            var ds = new DocumentStore { ConnectionStringName = "db" };
            ds.Initialize(true);
            return ds;
        }
    }
}