using System.Web.Mvc;
using Demo.UI;
using Demo.UI.IoC;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using StructureMap;
using WebActivatorEx;
using System.IO;

[assembly: PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]
[assembly: ApplicationShutdownMethod(typeof(StructuremapMvc), "End")]

namespace Demo.UI
{
    public static class StructuremapMvc
    {
        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        public static void End()
        {
            StructureMapDependencyScope.Dispose();
        }

        public static void Start()
        {
            IContainer container = IoC.IoC.Initialize();
            StructureMapDependencyScope = new StructureMapDependencyScope(container);
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            ServiceLocator.SetLocatorProvider(() => StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof (StructureMapScopeModule));
            //File.WriteAllText(@"C:\Users\aneta\Documents\Visual Studio 2013\Projects\RavenDemo\ioc.txt", container.WhatDoIHave());
        }
    }
}