using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;

namespace Routing
{
    public class Routing : IRouting {
        HttpClient client;
        List<Station> stations;
        public Routing() {
            this.client = new HttpClient();
            this.GetAllStation();
        }
        public RoutingResult GetRoutingMap(string start, string end) {
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            RoutingResult errorResult = new RoutingResult();
            System.Diagnostics.Debug.WriteLine("He wants to go from " + start + " to " + end);
            if(stations == null || stations.Count() < 2) {
                errorResult.infos = "Error, there is no station available at the moment, try another time";
                return errorResult;
            }
            Position startCo = getPositionFromAddress(start);
            if (startCo == null) {
                errorResult.infos = "Error, no coordinates for your starting address " + start + " has been found";
                return errorResult;
            }
            Position endCo = getPositionFromAddress(end);
            if (endCo == null) { 
                errorResult.infos = "Error, no coordinates for your destination address " + end + " has been found";
                return errorResult;
            }
            if(startCo.Equals(endCo)) {
                errorResult.infos = "The start and the destination are at the same place.";
                return errorResult;
            }
            System.Diagnostics.Debug.WriteLine("(" + startCo.latitude + ", " + startCo.longitude + ") -> (" + endCo.latitude + ", " + endCo.longitude + ")");
           
            Station stationStart = nearestAvailableStation(startCo, true);
            if (stationStart == null) {
                errorResult.infos = "Sorry but there is no stations with available bike for the moment, try later.";
                return errorResult;
            }
            Station stationEnd = nearestAvailableStation(endCo, false);
            if (stationStart == null) {
                errorResult.infos = "Sorry but there is no stations with available stands to leave the bike for the moment, try later.";
                return errorResult;
            }
            List<string> routeWalking = GetRoutesFromGPS(startCo, null, null, endCo);
            if (stationStart.Equals(stationEnd)){
                RoutingResult rW = new RoutingResult();
                rW.routes = routeWalking;
                rW.infos = "OK";
                return rW;
            }
            List<string> routes = GetRoutesFromGPS(startCo, stationStart.position, stationEnd.position,endCo);
            if(routes == null || routeWalking == null) {
                errorResult.infos = "Sorry but there is no path at bicycle to your destination";
                return errorResult;
            }
            routes = checkTime(routes, routeWalking);
            RoutingResult r = new RoutingResult();
            r.routes = routes;
            r.infosStations = new List<Station>() { stationStart, stationEnd };
            r.infos = "OK";
            return r;
        }


     

        /**
         * Call webProxy to get all the station at the beginning
         **/ 
        public List<Station> GetAllStation() {
            try {
                string request = "https://api.jcdecaux.com/vls/v3/stations?apiKey=ac428b37563fe08de2eeea03fe75f27e4dce458a";
                string s = getResult(request).Result;
                this.stations = JsonConvert.DeserializeObject<List<Station>>(s);
                System.Diagnostics.Debug.WriteLine("All the stations were recuperated");
                return stations;
            }
            catch(Exception e) {
                System.Diagnostics.Debug.WriteLine("Error while recupering stations");
                return null;
            }
        }

        /**
         * Call the cache to have the information about one station.
         */
        public Station GetInfosStation(string contractName, string stationNb) {
            try {
                string request = "http://localhost:8733/Design_Time_Addresses/Cache/GetStationInfos?stationNb=" + stationNb + "&contractName=" + contractName;
                string st = JsonConvert.DeserializeObject<GetStationInfoResultClass>(getResult(request).Result).GetStationInfoResult;
                return JsonConvert.DeserializeObject<Station>(st);
            }
            catch(Exception e) {
                System.Diagnostics.Debug.WriteLine("Error while getting infos on station : " + stationNb +" of " + contractName);
                return null;
            }
        }

        /**
         *  Call the mapbox direction api to get the route between 4 points
         **/
        public List<string> GetRoutesFromGPS(Position start, Position station1, Position station2, Position end) {
            try {
                if (station1 == null || station2 == null) { 
                    string route = "https://api.mapbox.com/directions/v5/mapbox/walking/" + start.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + start.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + ";" + end.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + end.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "?alternatives=false&geometries=geojson&steps=true&access_token=pk.eyJ1IjoibHVjYXNwb2x5dGVjaCIsImEiOiJja25lazgxemYyNjZ6MnVtcWNuY2ltMTU5In0.PRHFI72a0818EQvpX_VLzA";
                    System.Diagnostics.Debug.WriteLine(route);
                    return new List<string>() { getResult(route).Result};
                }
                string goToStation1 = "https://api.mapbox.com/directions/v5/mapbox/walking/" + start.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + start.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + ";" + station1.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + station1.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "?alternatives=false&geometries=geojson&steps=true&access_token=pk.eyJ1IjoibHVjYXNwb2x5dGVjaCIsImEiOiJja25lazgxemYyNjZ6MnVtcWNuY2ltMTU5In0.PRHFI72a0818EQvpX_VLzA";
                string bikeRoute = "https://api.mapbox.com/directions/v5/mapbox/cycling/" + station1.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + station1.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + ";" + station2.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + station2.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "?alternatives=false&geometries=geojson&steps=true&access_token=pk.eyJ1IjoibHVjYXNwb2x5dGVjaCIsImEiOiJja25lazgxemYyNjZ6MnVtcWNuY2ltMTU5In0.PRHFI72a0818EQvpX_VLzA";
                string leaveBike = "https://api.mapbox.com/directions/v5/mapbox/walking/" + station2.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + station2.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + ";" + end.longitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "," + end.latitude.ToString(CultureInfo.CreateSpecificCulture("en-EN")) + "?alternatives=false&geometries=geojson&steps=true&access_token=pk.eyJ1IjoibHVjYXNwb2x5dGVjaCIsImEiOiJja25lazgxemYyNjZ6MnVtcWNuY2ltMTU5In0.PRHFI72a0818EQvpX_VLzA";
                System.Diagnostics.Debug.WriteLine(goToStation1);
                System.Diagnostics.Debug.WriteLine(bikeRoute);
                System.Diagnostics.Debug.WriteLine(leaveBike);
                string route1 = getResult(goToStation1).Result;
                string route2 = getResult(bikeRoute).Result;
                string route3 = getResult(leaveBike).Result;
                return new List<string>() { route1, route2, route3 };
            }
            catch {
                System.Diagnostics.Debug.WriteLine("Error while getting the routes");
                return null;
            }
        }

        /**
         * Call geocoding api to find the coordinate of a address (any format of string)
         **/
        public async Task<string> getResultFromGeoCoding(string address) {
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

        /**
         * return the coordinate of an address on earth.
         **/
        public Position getPositionFromAddress(string address) {
            try {
                string s = getResultFromGeoCoding(address).Result;
                PositionGeoCoord p = JsonConvert.DeserializeObject<List<PositionGeoCoord>>(s)[0];
                return new Position(p.lat, p.lon);
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("Error while getting coordinates");
                return null;
            }
        }

        /**
         * Search in all the station one by one, the nearest with available bike(pickup=true)/or available space (pickup=false)
         **/
         public Station nearestAvailableStation(Position startCo, bool pickup) {           
            List<Station> ignore = new List<Station>();
            bool stationFound = false;
            Station station;
            do {
                station = nearestStation(startCo, ignore);
                if (station == null) {
                    System.Diagnostics.Debug.WriteLine("There is no station with available bike anywhere");
                    return null;
                }
                Station stationRes = GetInfosStation(station.contractName, station.number);
                if(stationRes == null) {
                    System.Diagnostics.Debug.WriteLine("Internal Error");
                    ignore.Add(station);
                    continue;
                }
                station = stationRes;
                if (pickup && station.totalStands.availabilities.bikes == 0) {
                    System.Diagnostics.Debug.WriteLine("Here1");
                    ignore.Add(station);
                }
                else if(!pickup && station.totalStands.availabilities.stands == 0) {
                    System.Diagnostics.Debug.WriteLine("Here2");
                    ignore.Add(station);
                    }
                else {
                    System.Diagnostics.Debug.WriteLine("Here3");
                    stationFound = true;
                    }
            }
            while (!stationFound);
            return station;
        }

        /**
         * Find the nearest station of a coordinate, the station in the ignore list are ignored
         **/
        public Station nearestStation(Position position, List<Station> ignore) {
            Station nearest = null;
            foreach(Station s in stations) {
                if (!ignore.Contains(s)) {
                    nearest = s;
                    break;
                }

            }
            if (nearest == null)
                return null;
            double distMin = double.MaxValue;
            foreach (Station station in stations) {
                if (!ignore.Contains(station)) {
                    double distance = getDistance(position, station.position);
                    if (distance < distMin) {
                        distMin = distance;
                        nearest = station;
                    }
                }
            }
            return nearest;
        }

        public List<string> checkTime(List<string> routes, List<string> routeWalking) {
            try {
                if (routes.Count < 2)
                    return routes;
                int deb0 = routes[0].IndexOf("duration\":") + 10;
                int fin0 = routes[0].IndexOf(",\"distance");
                string t0 = routes[0].Substring(deb0, fin0 - deb0);
                int deb1 = routes[1].IndexOf("duration\":") + 10;
                int fin1 = routes[1].IndexOf(",\"steps");
                string t1 = routes[1].Substring(deb1, fin1 - deb1);
                int deb2 = routes[2].IndexOf("duration\":") + 10;
                int fin2 = routes[2].IndexOf(",\"distance");
                string t2 = routes[2].Substring(deb2, fin2 - deb2);
                int debW = routeWalking[0].IndexOf("duration\":") + 10;
                int finW = routeWalking[0].IndexOf(",\"distance");
                string tW = routeWalking[0].Substring(debW, finW - debW);
                System.Diagnostics.Debug.WriteLine(t0 + " " + t1 + " "  + t2 + " " + tW);
                if (float.Parse(t0, CultureInfo.InvariantCulture) + float.Parse(t1, CultureInfo.InvariantCulture) + float.Parse(t2, CultureInfo.InvariantCulture) >= float.Parse(tW, CultureInfo.InvariantCulture))
                    return routeWalking;
                return routes;
            }
            catch {
                System.Diagnostics.Debug.WriteLine("Error while computing time");
                return routes;
            }

        }

        /**
         * return the response Task of an http request. 
         **/
        public async Task<string> getResult(string request) {
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /**
         * Methods to compute the distance between to coordinate on earth (given in TD)
        **/
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

        double deg2rad(double deg) {
            return deg * (Math.PI / 180);
        }
    }
}
