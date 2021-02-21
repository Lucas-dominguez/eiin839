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
            start.FileName = @"D:\LUCAS\Polytech\SI4\2eSemestre\Soc\TD\Fork\eiin839\TD2\WebDynamic\ExecMethod\bin\Debug\ExecMethod.exe";
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


        public string myMethod3(string param1, string param2)
        {
            // Question 6 :

            string fileName = @"D:\LUCAS\Polytech\SI4\2eSemestre\Soc\TD\Fork\eiin839\TD2\WebDynamic\execPython.py" + " " + param1 + " " + param2;
            string interpret = @"D:\LUCAS\Programmes\Python\python.exe";
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(interpret, fileName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();

            return output;
        }

    }
}
