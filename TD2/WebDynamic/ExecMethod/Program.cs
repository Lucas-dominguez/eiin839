using System;

namespace ExecMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >0)
                Console.WriteLine("<HTML><BODY> Hello from executable : " + args[0] + ", " + args[1] + "</BODY></HTML>");
            else
                Console.WriteLine("Pas d'arguments");
        }
    }
}
