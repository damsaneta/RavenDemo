using System;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace Demo.UI.IoC
{
    public class ControllerConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.CanBeCastTo<Controller>() && !type.IsAbstract)
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }
        }

        public void ScanTypes(TypeSet types, Registry registry)
        {
            var all = types.AllTypes().Where(t => !t.IsAbstract && t.CanBeCastTo<Controller>()).ToList();
            all.ForEach(t => registry.For(t).LifecycleIs(new UniquePerRequestLifecycle()));
           // registry.Policies.SetAllProperties(x => x.OfType<>());
        }
    }
}