using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using heavyClient.ServiceReferenceRouting;
namespace heavyClient {
    class Program {
        static void Main(string[] args) {
            RoutingClient routing = new RoutingClient("SoapRouting");
            Console.WriteLine("Welcome to Let's go Biking ! \n Here you can search for the fastest route using the bicycle of JCDecaux");
            Console.WriteLine("Please press enter to begin a research");
            Console.ReadLine();
            Console.WriteLine("Enter your starting location : ");
            string start = Console.ReadLine();
            Console.WriteLine("Enter your destination : ");
            string end = Console.ReadLine();
            List<Station> stations = routing.GetRouting(start, end).ToList();
            Console.WriteLine("You have to go to : ");
            foreach (Station s in stations) {
                Console.WriteLine(s.name + "located at " + s.address);
            }
            Console.ReadLine();
        }
    }
}
