using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using heavyClient.ServiceReferenceRouting;
using Newtonsoft.Json;

namespace heavyClient {
    class Program {
        static void Main(string[] args) {
            RoutingClient routing = new RoutingClient("SoapRouting");
            Console.WriteLine("Welcome to Let's go Biking ! \n Here you can search for the fastest route using the bicycle of JCDecaux");
            Console.WriteLine("Please press enter to begin");
            Console.ReadLine();
            string res;
            do {
                Console.WriteLine("Select what do you want to do :");
                Console.WriteLine("     Search for a route -----> 1");
                Console.WriteLine("     View uses of stations -----> 2");
                Console.WriteLine("     Exit -----> 3");
                res = Console.ReadLine();
                if(res == "1") {
                    RoutingResult r = searchRouting(routing);
                    displayResult(r);
                }
                else if(res == "2")
                {
                    Console.WriteLine("Stats");
                }
                else if(res!="3")
                {
                    Console.WriteLine("Error bad input try again");
                }
            }
            while (res != "3");
            Console.WriteLine("Goodbye\nApplication closing...");
            Console.ReadLine();
        }
        static RoutingResult searchRouting(RoutingClient routing)
        {
            Console.WriteLine("Enter your starting location : ");
            string start = Console.ReadLine();
            Console.WriteLine("Enter your destination : ");
            string end = Console.ReadLine();
            Console.WriteLine("Waiting .... ");
            return routing.GetRoutingMap(start, end);
        }

        static void displayResult(RoutingResult r) {
            if (r.infos != "OK") {
                Console.WriteLine("Sorry there have been an error with your request please try again or later");
                return;
            }
            List<Route> routes = new List<Route>();
            List<Station> stations = r.infosStations.ToList();
            foreach (string s in r.routes.ToList()) {
                routes.Add(JsonConvert.DeserializeObject<RouteResult>(s).routes[0]);
            }
            if (routes.Count > 2) {
                Console.WriteLine("You have to go to take the bike at the station " + stations[0].name + " located at " + stations[0].address);
                Console.WriteLine("Distance : " + routes[0].distance + " km (estimate time " + Math.Round(float.Parse(routes[0].duration, CultureInfo.InvariantCulture)/60) + " min )\n");
                Console.WriteLine("\n");
                Console.WriteLine("Then you will have to drive to the station " + stations[1].name + " located at " + stations[1].address + " to leave the bike.");
                Console.WriteLine("Distance : " + routes[1].distance + " km (estimate time " + Math.Round(float.Parse(routes[1].duration, CultureInfo.InvariantCulture) / 60) + " min )\n");
                Console.WriteLine("Finally you can walk to your destination");
                Console.WriteLine("Distance : " + routes[2].distance + " km (estimate time " + Math.Round(float.Parse(routes[2].duration, CultureInfo.InvariantCulture) / 60) + " min )\n");
            }
            else {
                Console.WriteLine("Sorry you have to do the trip by walking");

            }
            string res;
            do {
                Console.WriteLine("Do you want to ..");
                Console.WriteLine("See details on the path ------> 1");
                Console.WriteLine("See infos on the stations ----> 2 ");
                Console.WriteLine("Go back ----> 3 ");
                res = Console.ReadLine();
                if (res == "1") {
                    //displayPath(routes);
                }
                else if (res == "2") {
                    //diplayStations(stations);
                }
                else if (res != "3")
                {
                    Console.WriteLine("Error bad input try again");
                }
            } while (res != "3");
        }


        static void displayPath(List<Route> routes)
        {

        }
    }


}
