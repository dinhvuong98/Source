using Microsoft.Extensions.Options;
using Services.Interfaces.RedisCache;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using Utilities.Common.Dependency;
using Utilities.Configurations;

namespace Services.Implementation.RedisCache
{
    public class RedisCacheProvider : ICacheProvider
    {
        RedisEndpoint _endPoint;
        private readonly RedisCacheSettings redisCacheSettings = SingletonDependency<IOptions<RedisCacheSettings>>.Instance.Value;
        public RedisCacheProvider()
        {
            _endPoint = new RedisEndpoint(redisCacheSettings.Host, int.Parse(redisCacheSettings.Port), redisCacheSettings.PassWord, long.Parse(redisCacheSettings.DatabaseID));
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {
            Set(key, value, TimeSpan.Zero);
        }

        public void Set<T>(string key, T value, TimeSpan timeout)
        {
            using (RedisClient client = new RedisClient(_endPoint))
            {
                client.As<T>().SetValue(key, value, timeout);
            }
        }

        /// <summary>
        /// GetByKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetByKey<T>(string key)
        {
            T result = default(T);

            using (RedisClient client = new RedisClient(_endPoint))
            {
                var wrapper = client.As<T>();

                result = wrapper.GetValue(key);
            }

            return result;
        }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetAll<T>()
        {

            using (RedisClient client = new RedisClient(_endPoint))
            {
                return client.As<T>().GetAll();
            }

        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            bool removed = false;

            using (RedisClient client = new RedisClient(_endPoint))
            {
                removed = client.Remove(key);
            }

            return removed;
        }

        /// <summary>
        /// IsInCache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsInCache(string key)
        {
            bool isInCache = false;

            using (RedisClient client = new RedisClient(_endPoint))
            {
                isInCache = client.ContainsKey(key);
            }

            return isInCache;
        }

        /// <summary>
        /// DeleteById
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public void DeleteById<T>(object id)
        {
            using (RedisClient client = new RedisClient(_endPoint))
            {
                client.As<T>().DeleteById(id);
            }
        }
        /// <summary>
        /// StoreAll
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        public void StoreAll<T>(IEnumerable<T> e)
        {
            using (RedisClient client = new RedisClient(_endPoint))
            {
                client.As<T>().StoreAll(e);
            }
        }


    }
}
