using System;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cache
{
    public class ProxyCache : IProxyCache
    {
        DateTimeOffset dt_default;
        ObjectCache cache;
        HttpClient client;

        public ProxyCache()
        {
            this.cache = MemoryCache.Default;
            this.dt_default = ObjectCache.InfiniteAbsoluteExpiration;
            this.client = new HttpClient();
        }
        public List<JCDecauxItem.Station> Get(string request)
        {
            List<JCDecauxItem.Station> obj = (List<JCDecauxItem.Station>) this.cache.Get(request);
            if(obj == null){
                obj = getResult(request).Result;
                this.cache.Add(request, obj, dt_default);
            }
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return obj;
        }

        public List<JCDecauxItem.Station> Get(string request, double expirationTime)
        {
            List<JCDecauxItem.Station> obj = (List<JCDecauxItem.Station>)this.cache.Get(request);
            if (obj == null)
            {
                obj = getResult(request).Result;
                this.cache.Add(request, obj, DateTimeOffset.Now.AddSeconds(expirationTime));
            }
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return obj;
        }

        public List<JCDecauxItem.Station> Get(string request, DateTimeOffset expirationTime)
        {
            List<JCDecauxItem.Station> obj = (List<JCDecauxItem.Station>)this.cache.Get(request);
            if (obj == null)
            {
                obj = getResult(request).Result;
                this.cache.Add(request, obj, expirationTime);
            }
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return obj;
        }

        public String PrintAllCache()
        {
            StringBuilder s = new StringBuilder("\nTime : " + DateTime.Now, 100);
            foreach (var item in this.cache){
                s.Append(item.Key + "-" + item.Value);
            }
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            return s.ToString();
        }

        public async Task<List<JCDecauxItem.Station>> getResult(String request)
        {
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<JCDecauxItem.Station>>(responseBody);
        }

    }
}
