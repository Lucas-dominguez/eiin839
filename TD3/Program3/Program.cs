using System;
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
                if (args.Length != 2)
                {
                    Console.WriteLine("Mauvais nombre d'arguments");
                    System.Environment.Exit(-1);
                }
                string stationNumber = args[0];
                string contractName = args[1];
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v1/stations/"+ stationNumber + "?contract=" + contractName  + "&apiKey=ac428b37563fe08de2eeea03fe75f27e4dce458a");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

    }
}
