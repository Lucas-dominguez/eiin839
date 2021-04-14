﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Routing
{
    [ServiceContract]
    public interface IRouting
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "route?start={start}&end={end}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<Station> GetRouting(string start, string end);

        [WebInvoke(UriTemplate = "mapRoute?start={start}&end={end}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        RoutingResult GetRoutingMap(string start, string end);


        }


    }
