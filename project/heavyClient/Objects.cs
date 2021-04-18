
using System;
using System.Collections.Generic;
namespace heavyClient
{


    public class RouteResult
    {
        public List<Route> routes { get; set; }
    }
    public class Route
    {
        public string weight_name { get; set; }
        public string duration { get; set; }
        public string distance { get; set; }
        public List<Leg> legs { get; set; }
    }
    public class Leg
    {
        public List<Step> steps { get; set; }
    }
    public class Step
    {
        public Maneuver maneuver { get; set; }
    }

    public class Maneuver
    {
        public string instruction { get; set; }
    }

}