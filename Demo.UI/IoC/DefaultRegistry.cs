using Demo.ApplicationLogic;
using Demo.Domain.Orders;
using Demo.Domain.Shared;
using Demo.Storage.Infrastructure;
using Demo.Storage.Repositories;
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
                    scan.AssemblyContainingType<ICartRepository>();
                    scan.AssemblyContainingType<ICartService>();
					scan.With(new ControllerConvention());
                    scan.ConnectImplementationsToTypesClosing(typeof(IRepository<>)).OnAddedPluginTypes(x => x.Singleton());
                    scan.WithDefaultConventions().OnAddedPluginTypes(x => x.Singleton());
                });
        }

        private static DocumentStore CreateDocumentStore()
        {
            var ds = new DocumentStore { ConnectionStringName = "db" };
            ds.Initialize(true);
            return ds;
        }
    }
}