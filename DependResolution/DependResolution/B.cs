using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependResolution
{
    public class B : IB
    {
        public B(IA a) { }
        public void checkB()
        {
            Console.WriteLine("B");
        }
    }
}
