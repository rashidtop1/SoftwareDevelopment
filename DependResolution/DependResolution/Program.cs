using System;

namespace DependResolution
{
    class Program
    {
        static void Main(string[] args)
        {
            DI_Container container = new DI_Container();
            container.AddTransient<IB, A>();
            container.AddTransient<IA, B>();
            container.Get<IA>();
        }
    }
}
