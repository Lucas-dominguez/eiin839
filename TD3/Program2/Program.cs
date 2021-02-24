using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JCDecaux
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Mauvais nombre d'arguments");
                    System.Environment.Exit(-1);
                }
                string contractName = args[0];
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v1/stations?contract="+contractName+ "&apiKey=ac428b37563fe08de2eeea03fe75f27e4dce458a");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);
                var stations = JsonConvert.DeserializeObject<List<Station>>(responseBody);
                foreach (Station element in stations)
                {
                    Console.WriteLine(element.number);
                }


            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

    }
    public class Station
    {
        public string number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public List<string> positions { get; set; }
        public Boolean banking { get; set; }
        public Boolean bonus { get; set; }
        public int bike_stands { get; set; }
        public int available_bike_stands { get; set; }
        public string status { get; set; }
        public string last_update { get; set; }
    }

}
