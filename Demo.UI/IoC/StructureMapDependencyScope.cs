using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace Demo.UI.IoC
{
    /// <summary>
    /// The structure map dependency scope.
    /// </summary>
    public class StructureMapDependencyScope : ServiceLocatorImplBase
    {
        private const string NestedContainerKey = "Nested.Container.Key";

        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        public IContainer Container { get; set; }

        public IContainer CurrentNestedContainer
        {
            get { return (IContainer) HttpContext.Items[NestedContainerKey]; }
            set { HttpContext.Items[NestedContainerKey] = value; }
        }

        private HttpContextBase HttpContext
        {
            get
            {
                var ctx = Container.TryGetInstance<HttpContextBase>();
                return ctx ?? (System.Web.HttpContext.Current == null ? null : new HttpContextWrapper(System.Web.HttpContext.Current));
            }
        }

        public void CreateNestedContainer()
        {
            if (CurrentNestedContainer != null)
            {
                return;
            }

            CurrentNestedContainer = Container.GetNestedContainer();
        }

        public void Dispose()
        {
            DisposeNestedContainer();
            Container.Dispose();
        }

        public void DisposeNestedContainer()
        {
            if (HttpContext == null)
            {
                return;
            }

            if (CurrentNestedContainer != null)
            {
                CurrentNestedContainer.Dispose();
                CurrentNestedContainer = null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return DoGetAllInstances(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (CurrentNestedContainer ?? Container).GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            IContainer container = (CurrentNestedContainer ?? Container);

            if (string.IsNullOrEmpty(key))
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                    ? container.TryGetInstance(serviceType)
                    : container.GetInstance(serviceType);
            }

            return container.GetInstance(serviceType, key);
        }
    }
}
