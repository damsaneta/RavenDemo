using StructureMap;

namespace Demo.UI.IoC 
{
    public static class IoC 
    {
        public static IContainer Initialize() 
        {
            return new Container(c => c.AddRegistry<DefaultRegistry>());
        }
    }
}