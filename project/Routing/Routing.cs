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
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            System.Diagnostics.Debug.WriteLine("Il veut aller de "+ start + " à " + end);
            Position startCo = getPositionFromAddress(start);
            Position endCo = getPositionFromAddress(end);
            System.Diagnostics.Debug.WriteLine("(" + startCo.latitude + ", " + startCo.longitude + ") -> (" + endCo.latitude + ", " + endCo.longitude + ")");
            Station stationStart = nearestStation(startCo);
            Station stationEnd = nearestStation(endCo);
            stationStart = GetInfosStation(stationStart.contractName, stationStart.number);
            stationEnd = GetInfosStation(stationEnd.contractName, stationEnd.number);
            return new List<Station>() { stationStart, stationEnd };
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


        public async Task<string> getResult(string request) {
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> getResultFromGeoCoding(string address)
        {
            string request = "https://forward-reverse-geocoding.p.rapidapi.com/v1/search?q=" + address + "&format=json&accept-language=en&polygon_threshold=0.0";
            var r = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(request),
                Headers = {
                    { "x-rapidapi-key", "b82306176amsh10f7b990e3d70bep1ba279jsn7ed83c293ebb" },
                    { "x-rapidapi-host", "forward-reverse-geocoding.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(r))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }

        public Position getPositionFromAddress(string address) {
            string s = getResultFromGeoCoding(address).Result;
            PositionGeoCoord p = JsonConvert.DeserializeObject<List<PositionGeoCoord>>(s)[0];
            return new Position(p.lat, p.lon);
        }

        public Station nearestStation(Position position) {
            Station nearest = this.stations[0];
            double distMin = double.MaxValue;
            foreach (Station station in stations){
                double distance = getDistance(position, station.position);
                if (distance < distMin) {
                    distMin = distance;
                    nearest = station;
                }
            }
            return nearest;
        }

        double getDistance(Position pos1, Position pos2) {
            var earthRadius = 6371;
            var dLat = deg2rad(pos2.latitude - pos1.latitude);
            var dLon = deg2rad(pos2.longitude - pos1.longitude);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(deg2rad(pos1.latitude)) * Math.Cos(deg2rad(pos2.latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = earthRadius * c; 
            return d;
        }

        double deg2rad(double deg){
            return deg * (Math.PI / 180);
        }
    }
}
