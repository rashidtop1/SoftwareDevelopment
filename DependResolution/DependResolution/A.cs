using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependResolution
{
    public class A : IA
    {
        public A(IB b) { }
        public void checkA()
        {
            Console.WriteLine("A");
        }
    }
}
