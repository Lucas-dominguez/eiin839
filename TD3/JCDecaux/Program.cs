using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JCDecaux
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v1/contracts?apiKey=ac428b37563fe08de2eeea03fe75f27e4dce458a");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var contracts = JsonConvert.DeserializeObject<List<Contract>>(responseBody);
                foreach (Contract element in contracts)
                {
                    Console.WriteLine(element.name);
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

    }
    public class Contract
    {
        public string name { get; set; }
        public string commercialName { get; set; }
        public List<string> cities { get; set; }
        public string countryCode { get; set; }
    }
}
