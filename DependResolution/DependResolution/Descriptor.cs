using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependResolution
{
    class Descriptor
    {

        public Descriptor(Type serviceType, Type implementationType, ServiceLifeTime lifeTime)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            LifeTime = lifeTime;
        }
        public Type ServiceType {
            get; 
        }

        public Type ImplementationType { 
            get; 
        }

        public object Implementation {
            get; 
            internal set; 
        }

        public ServiceLifeTime LifeTime { 
            get;
        }

        
    }
}

