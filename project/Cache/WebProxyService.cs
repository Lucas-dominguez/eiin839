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
            System.Diagnostics.Debug.WriteLine("On me demande : " + stationNb  +" à " + contractName);
            string s = cache.Get(new List<string>() { stationNb, contractName }, 300).JCDecauxContent;
            System.Diagnostics.Debug.WriteLine("OK done ! ");
            return s; // Delai d'expiration du cache
        }

        public string PrintAllCache(){
            return cache.PrintAllCache();
        }


    }



}
