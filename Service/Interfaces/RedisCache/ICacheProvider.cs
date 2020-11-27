using System;
using System.Collections.Generic;

namespace Services.Interfaces.RedisCache
{
    public interface ICacheProvider
    {
        void Set<T>(string key, T value);

        void Set<T>(string key, T value, TimeSpan timeout);

        T GetByKey<T>(string key);

        IList<T> GetAll<T>();

        bool Remove(string key);

        void DeleteById<T>(object id);

        bool IsInCache(string key);

        void StoreAll<T>(IEnumerable<T> e);

    }
}
