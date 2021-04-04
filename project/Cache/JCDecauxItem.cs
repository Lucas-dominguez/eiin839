using System;

public class JCDecauxItem
{
    public class Station
    {
        public string number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public int bike_stands { get; set; }
        public int available_bike_stands { get; set; }
        public string status { get; set; }
        public string last_update { get; set; }

        public Station()
        {
            Console.WriteLine("Instenciation of a station");
        }
    }

    public class Position{
        public float lat { get; set; }
        public float lng { get; set; }
    }
}
