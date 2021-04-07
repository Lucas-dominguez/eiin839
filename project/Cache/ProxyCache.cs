using System;
using System.Runtime.Caching;
using System.Text;
using System.Collections.Generic;

namespace Cache
{
    public class ProxyCache<T> where T : CachableClass, new()
    {
        DateTimeOffset dt_default;
        ObjectCache cache;
        public string idSeperator = "&&&&&&&&&&&&";

        public ProxyCache()
        {
            this.cache = MemoryCache.Default;
            this.dt_default = ObjectCache.InfiniteAbsoluteExpiration;
        }
        public T Get(List<string> ids)
        {
            string id = String.Join(idSeperator, ids.ToArray());
            T obj = (T)this.cache.Get(id);
            if (obj == null)
            {
                obj = new T();
                obj.instantiate(ids);
                this.cache.Add(id, obj, dt_default);
            }
            return obj;
        }

        public T Get(List<string> ids, double expirationTime)
        {
            string id = String.Join(idSeperator, ids.ToArray());
            T obj = (T)this.cache.Get(id);
            if (obj == null)
            {
                obj = new T();
                obj.instantiate(ids);
                this.cache.Add(id, obj, DateTimeOffset.Now.AddSeconds(expirationTime));
            }
            return obj;
        }

        public T Get(List<string> ids, DateTimeOffset expirationTime)
        {
            string id = String.Join(idSeperator, ids.ToArray());
            T obj = (T)this.cache.Get(id);
            if (obj == null)
            {
                obj = new T();
                obj.instantiate(ids);
                this.cache.Add(id, obj, expirationTime);
            }
            return obj;
        }

        public string PrintAllCache()
        {
            StringBuilder s = new StringBuilder("\nTime : " + DateTime.Now, 100);
            foreach (var item in this.cache)
            {
                s.Append(item.Key + "-" + item.Value);
            }
            return s.ToString();
        }
    }

    public abstract class CachableClass{
        public CachableClass() {}

        public abstract void instantiate(List<string> ids);
    }
}