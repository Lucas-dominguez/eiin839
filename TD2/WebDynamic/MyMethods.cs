using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace WebDynamicUtils
{
    class MyMethods
    {
        //Question 4 :
       public string myMethod1(string param1, string param2)
        {
            return "Hello : " + param1 + ", " + param2;
        }

        public string myMethod2(string param1, string param2)
        {
            // Question 5 :
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"D:\LUCAS\Polytech\SI4\2e Semestre\Soc\TD\Fork\eiin839\TD2\WebDynamic\ExecMethod\bin\Debug\ExecMethod.exe";
            start.Arguments = param1 + " " + param2;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }

    }
}
