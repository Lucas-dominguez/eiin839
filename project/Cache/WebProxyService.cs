using System;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cache
{
    public class WebProxyService : IWebProxyService{
        ProxyCache<JCDecauxItem> cache;
        WebProxyService(){
            cache = new ProxyCache<JCDecauxItem>();
        }

        public string GetStationInfo(string stationNb, string contractName){
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return cache.Get(new List<string>() { stationNb, contractName }, 60).JCDecauxContent;
        }

        public string PrintAllCache(){
            return cache.PrintAllCache();
        }


    }



}
