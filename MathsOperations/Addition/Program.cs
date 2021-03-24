using System;
namespace Addition
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServiceReference1.MathsOperationsClient m = new ServiceReference1.MathsOperationsClient();
            ServiceReference2.MathsOperationsClient m1 = new ServiceReference2.MathsOperationsClient("SoapEnd1");
            ServiceReference2.MathsOperationsClient m2 = new ServiceReference2.MathsOperationsClient("SoapEnd2");
            Console.WriteLine(m1.Add(1, 2));
            Console.WriteLine(m1.Multiply(2,3));
            Console.WriteLine(m1.Subtract(1, 10));
            Console.WriteLine(m1.Divide(8, 3));
            Console.WriteLine(m2.Add(5, 5));
            Console.WriteLine(m2.Multiply(5, 5));
            Console.WriteLine(m2.Subtract(5, 5));
            Console.WriteLine(m2.Divide(5, 5));
            Console.ReadLine();
        }
    }
}
