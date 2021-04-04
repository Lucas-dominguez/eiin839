using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Web;

namespace Cache
{
    [ServiceContract]
    public interface IProxyCache
    {
        [OperationContract(Name ="GetById")]
        [WebInvoke(UriTemplate = "Get?request={request}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<JCDecauxItem.Station> Get(string request);
        [OperationContract(Name ="GetDelaySec")]
        [WebInvoke(UriTemplate = "GetDelay?request={request}&expirationTime={expirationTime}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<JCDecauxItem.Station> Get(string request, double expirationTime);
        [OperationContract(Name ="GetDelay")]
        [WebInvoke(UriTemplate = "GetDelay2?request={request}&expirationTime={expirationTime}", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<JCDecauxItem.Station> Get(string request, DateTimeOffset expirationTime);
        [OperationContract]
        [WebInvoke(UriTemplate = "PrintAllCache", Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        String PrintAllCache();

    }
}
