using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Routing
{

    public class Station
    {
        public string number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public string status { get; set; }
        public string last_update { get; set; }
        public bool connected { get; set; }
        public bool overflow { get; set; }
        public Stands totalStands { get; set; }
        public Stands mainStands { get; set; }
        public Stands overflowStands { get; set; }

        public Station() { }

        public Station(Position position, string name) {
            this.position = position;
            this.name = name;
        }
    }

    public class Stands
    {
        Availabilities availabilities { get; set; }
        int capacity { get; set; }
    }
    public class Availabilities
    {
        int bikes { get; set; }
        int stands { get; set; }
        int mechanicalBikes { get; set; }
        int electricalBikes { get; set; }
        int electricalInternalBatteryBikes { get; set; }
        int electricalRemovableBatteryBikes { get; set; }
    }
    [DataContractAttribute]
    public class Position
    {
        [DataMember]
        public float latitude { get; set; }
        [DataMember]
        public float longitude { get; set; }

        public Position(float latitude, float longitude) {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }

    public class PositionGeoCoord
    {
        public float lat { get; set; }
        public float lon { get; set; }
    }

    public class GetStationInfoResultClass {
        public string GetStationInfoResult { get; set; }
    }

    public class RoutingResult {
        public List<string> routes { get; set; }
        public List<Station> infosStations { get; set; }
        public string infos { get; set; }


    }

}