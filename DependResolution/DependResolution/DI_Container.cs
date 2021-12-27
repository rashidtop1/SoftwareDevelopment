﻿using System;
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


        public object Get(Type serviceType)
        {
            List<Type> listik = new List<Type>();

            return Get(serviceType, listik);
        }
        
        public object Get(Type serviceType, List<Type> parlist)
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

            List<object> Dishare = new List<object>();

             foreach (var parameter in construct.GetParameters())
               {
                if (parlist.Contains(serviceType))
                {
                    throw new Exception("Цикл.");
                }
                parlist.Add(serviceType);
                var newParameter = Get(parameter.ParameterType, parlist);
                parlist.Remove(serviceType);
                Dishare.Add(newParameter);
               }

            
            var parametr = construct.GetParameters().Select(x => Get(x.ParameterType)).ToArray();
            var implementation = Activator.CreateInstance(type, parametr);
            if (descriptor.LifeTime == ServiceLifeTime.Singleton)
            {
                descriptor.Implementation = implementation;
            }
            return implementation;
        }
    }
}

