using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebDynamicUtils
{
    class Header
    {
        NameValueCollection headers;
        public Header(NameValueCollection headers)
        {
            this.headers = headers;
        }
        
        public void PrintFieldHeader(HttpRequestHeader field)
        {
            Console.WriteLine(headers.Get(field.ToString()));
        }
        public void PrintAllHeader()
        {
            Console.WriteLine(headers.ToString());
        }
    }
}
