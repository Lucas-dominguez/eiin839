using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
namespace Cache
{
    public class JCDecauxItem : CachableClass
    {
        public string JCDecauxContent;
        public JCDecauxItem(){}

        public override void instantiate(List<string> ids){
            this.JCDecauxContent = sendRequest(ids[0], ids[1]).Result;
        }

        public async Task<string> sendRequest(string stationNumber, string contractName){
            string request = "https://api.jcdecaux.com/vls/v3/stations/"+ stationNumber + "?contract=" + contractName + "&apiKey=ac428b37563fe08de2eeea03fe75f27e4dce458a";
            System.Diagnostics.Debug.WriteLine("Requête envoyée à Decaux !!!!! : "+  request);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine("response : " + responseBody);
            return responseBody;
        }
    }


}