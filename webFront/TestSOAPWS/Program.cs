using System;

namespace TestSOAPWS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!\n");
            ServiceReference1.CalculatorSoap calculator = new ServiceReference1.CalculatorSoapClient(ServiceReference1.CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
            Console.WriteLine("2*2 : " + calculator.MultiplyAsync(2, 2).Result);
            Console.ReadLine();

        }
    }
}
