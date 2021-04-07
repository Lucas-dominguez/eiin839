using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;



namespace Routing
{
    public class Routing : IRouting
    {
        HttpClient client;
        List<Station> stations;
        public Routing(){
            this.client = new HttpClient();
            this.GetAllStation();
        }

        public List<Station> GetRouting(string start, string end){
            System.Diagnostics.Debug.WriteLine("Il veut aller de "+ start + " à " + end);
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            Station s = this.stations[0];
            s = GetInfosStation(s.contractName, s.number);
            return new List<Station>() { s };
        }

        public List<Station> GetAllStation() {
            string request = "https://api.jcdecaux.com/vls/v3/stations?apiKey=ac428b37563fe08de2eeea03fe75f27e4dce458a";
            string s = getResult(request).Result;
            //System.Diagnostics.Debug.WriteLine(s);
            this.stations = JsonConvert.DeserializeObject<List<Station>>(s);
            return stations;
        }

        public Station GetInfosStation(string contractName, string stationNb)
        {
            string request = "http://localhost:8733/Design_Time_Addresses/Cache/GetStationInfos?stationNb=" + stationNb + "&contractName=" + contractName;
            string st = JsonConvert.DeserializeObject<GetStationInfoResultClass>(getResult(request).Result).GetStationInfoResult;
            return JsonConvert.DeserializeObject<Station>(st);
        }


        public async Task<string> getResult(string request)
        {
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
