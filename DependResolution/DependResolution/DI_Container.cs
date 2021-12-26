using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependResolution
{
    class DI_Container
    {
        private List<Descriptor> dependencies;
        public DI_Container()
        {
            dependencies = new List<Descriptor>();
        }
        public void AddTransient<TService, TImplementation>()
        {
            dependencies.Add(new Descriptor(typeof(TService), typeof(TImplementation), ServiceLifeTime.Transient));
        }
        public void AddSingleton<TService, TImplementation>()
        {
            dependencies.Add(new Descriptor(typeof(TService), typeof(TImplementation), ServiceLifeTime.Singleton));
        }
        public T Get<T>() => (T)Get(typeof(T));
        public object Get(Type serviceType)
        {
            var descriptor = dependencies.SingleOrDefault(x => x.ServiceType == serviceType);
            if (descriptor == null)
            {
                throw new Exception("Сервис не найден");
            }
            if (descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }
            var type = descriptor.ImplementationType;
            var construct = type.GetConstructors().First();
            if (construct.GetParameters().Any(x => CheckCycled(serviceType, x.ParameterType)))
            {
                throw new Exception("Цикл");
            }
            var parametr = construct.GetParameters().Select(x => Get(x.ParameterType)).ToArray();
            var implementation = Activator.CreateInstance(type, parametr);
            if (descriptor.LifeTime == ServiceLifeTime.Singleton)
            {
                descriptor.Implementation = implementation;
            }
            return implementation;
        }
        public bool CheckCycled(Type serviceType, Type parametrType)
        {
            var descriptor = dependencies.SingleOrDefault(x => x.ServiceType == parametrType);
            var type = descriptor.ImplementationType;
            var ConstructType = type.GetConstructors().First();
            return ConstructType.GetParameters().Any(x => Equals(serviceType, x.ParameterType));
        }
    }
}

