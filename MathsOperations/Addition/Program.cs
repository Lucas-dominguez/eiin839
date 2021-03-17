using Addition.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addition
{
    class Program
    {
        static void Main(string[] args)
        {
            MathsOperationsClient m = new MathsOperationsClient();
            Console.WriteLine(m.Add(1, 2));
            Console.WriteLine(m.Multiply(2,3));
            Console.WriteLine(m.Subtract(1, 10));
            Console.WriteLine(m.Divide(8, 3));
            Console.ReadLine();
        }
    }
}
