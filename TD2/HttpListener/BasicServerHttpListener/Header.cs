using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Header
    {
        NameValueCollection headers;
        public Header(NameValueCollection headers)
        {
            this.headers = headers;
        }
        
        public void PrintHeader()
        {
            //Console.WriteLine(req.ToString());
            Console.WriteLine(headers.ToString());
        }
    }
}
