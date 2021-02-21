using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using WebDynamicUtils;
using System.Web;


namespace WebDynamic
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("A more recent Windows version is required to use the HttpListener class.");
                return;
            }


            HttpListener listener = new HttpListener();

            if (args.Length != 0)
            {
                foreach (string s in args)
                {
                    listener.Prefixes.Add(s);
                }
            }
            else
            {
                Console.WriteLine("Syntax error: the call must contain at least one web server url as argument");
            }
            listener.Start();
            foreach (string s in args)
            {
                Console.WriteLine("Listening for connections on " + s);
            }

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                Header h = new Header(request.Headers);

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }

                //Question 1 :
                //h.PrintFieldHeader(HttpRequestHeader.Accept);
                //h.PrintAllHeader();

                //Question 4 : http://localhost:8080/myMethod1?param1=Lucas&param2=DOMINGUEZ
                // Question 5 : http://localhost:8080/myMethod2?param1=Lucas&param2=DOMINGUEZ
                // Question 6 : http://localhost:8080/myMethod3?param1=Lucas&param2=DOMINGUEZ
                Type type = typeof(MyMethods);

                string methodName = "";
                foreach (string str in request.Url.Segments)
                {
                    methodName = str;
                }


                string result = "Hello World";
                if (!methodName.Equals("favicon.ico"))
                {
                    MethodInfo method = type.GetMethod(methodName);
                    MyMethods c = new MyMethods();
                    string param1 = HttpUtility.ParseQueryString(request.Url.Query).Get("param1");
                    string param2 = HttpUtility.ParseQueryString(request.Url.Query).Get("param2");
                    result = (string)method.Invoke(c, new string[] { param1, param2 });
                    Console.WriteLine(result);
                }
       

          

                string responseString = "<HTML><BODY> "+ result+" </BODY></HTML>";


                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                HttpListenerResponse response = context.Response;
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
    }
}