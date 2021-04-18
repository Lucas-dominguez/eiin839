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

        public override bool Equals(Object obj) {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
                return false;
            }
            else {
                Station s = (Station)obj;
                return (number == s.number) && (contractName == s.contractName);
            }
        }

        public override string ToString()
        {
            return number + ", " + contractName;
        }
    }

    public class Stands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }
    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
        public int electricalRemovableBatteryBikes { get; set; }
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

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Position p = (Position)obj;
                return (latitude == p.latitude) && (longitude == p.longitude);
            }
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