using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Web;

namespace Cache
{
    [ServiceContract]
    public interface IWebProxyService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "GetStationInfos?stationNb={stationNb}&contractName={contractName}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string GetStationInfo(string stationNb, string contractName);

        [OperationContract]
        [WebInvoke(UriTemplate = "PrintAllCache", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string PrintAllCache();

    }
}
