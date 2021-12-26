using System;

namespace SoftwareDevelopment
{
    class Program
    {
        static void Main(string[] args)
        {

            CustomDictionary<String, int> Dictionary = new CustomDictionary<string, int>();
            Dictionary.Add("Element1", 23);
         
            foreach (var elements in Dictionary)
            {
                Console.WriteLine(elements);
            }
        }
    }
}
