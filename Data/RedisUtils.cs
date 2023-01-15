

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;


namespace WikiPageApp.Data
{
    public class RedisUtils
    {
        //public static string ApplicationIDFromAppName = "ApplicationIDFromAppName";
        //public static string AppEnvIDFromAppIDAndEnvName = "AppEnvIDFromAppIDAndEnvName";
        //public static string ResourceIDFromSession = "ResourceIDFromSession";


        public static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            //var cacheConnection = "infinitecabinet.redis.cache.windows.net:6380,password=wti1YfN3Fc5rGkCdO7bMLCUJIGMT5OevEAzCaJ8cZNg=,ssl=True,abortConnect=False";
            var cacheConnection = ConfigUtils.GetRedisConfig("WikiPageApp");
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static readonly ConnectionMultiplexer Multiplexer = LazyConnection.Value;
        public static readonly IDatabase DB = Multiplexer.GetDatabase();


        public static string GetStringFromRedis(List<string> keySegments)
        {
            var redisKey = CreateKey(keySegments);
            return DB.StringGet(redisKey);
        }

        //todo: make sure key is less than 1024 chars 
        public static string CreateKey(List<string> keySegments) => String.Join("::", keySegments);

        public static void SaveLiteralToRedis(List<string> keySegments, object value, TimeSpan expiry)
        {
            var key = CreateKey(keySegments);
            SaveStringToRedis(key, value.ToString()!, expiry);
        }

        public static void SaveStringToRedis(string key, string value, TimeSpan expiry)
        {
            Task.Run(() =>
            {
                DB.StringSet(new RedisKey(key), new RedisValue(value), expiry, When.Always,
                    CommandFlags.FireAndForget);
            });
        }

        public static void SaveObjectToRedis(List<string> keySegments, object value, TimeSpan expiry)
        {
            var key = CreateKey(keySegments);
            SaveObjectToRedis(key, value, expiry);
        }

        public static void SaveObjectToRedis(string key, object value, TimeSpan expiry)
        {
            var serializedObject = JsonConvert.SerializeObject(value);
            DB.StringSet(new RedisKey(key), new RedisValue(serializedObject), expiry, When.Always,
                CommandFlags.FireAndForget);
        }

        public static List<string> GetKeysByPattern(string pattern)
        {
            var endpoints = LazyConnection.Value.GetEndPoints();
            var list = new List<string>();
            foreach (var endpoint in endpoints)
            {
                var server = LazyConnection.Value.GetServer(endpoint);
                var keys = server.Keys(pattern: "*" + pattern + "*").Select(x => x.ToString());
                list.AddRange(keys);
            }
            return list;
        }

        public static void WriteStringBatches(Dictionary<string, string> data, int batchSize = 100)
        {
            var batch = new List<KeyValuePair<RedisKey, RedisValue>>(batchSize);
            foreach (var pair in data)
            {
                batch.Add(new KeyValuePair<RedisKey, RedisValue>(pair.Key, pair.Value));
                if (batch.Count != batchSize) continue;
                DB.StringSet(batch.ToArray());
                batch.Clear();
            }
            if (batch.Count != 0) // final batch
                DB.StringSet(batch.ToArray());
        }

        public static void RemoveKey(string key)
        {
            DB.KeyDelete(key);
        }

        public static void RemoveKey(List<string> keySegments)
        {
            var key = CreateKey(keySegments);
            DB.KeyDelete(key);
        }


        public static string DeleteAllKeys()
        {
            var deleted = DB.Execute("DBSIZE");
            var results = DB.Execute("FLUSHALL");
            var remainingRecords = DB.Execute("DBSIZE");

            return $"{deleted} records deleted {results}, now {remainingRecords} records";
        }

        public static void DeleteBatchOfKeys(List<string> keys, int batchSize = 100)
        {
            var batch = new List<RedisKey>(batchSize);
            foreach (var key in keys)
            {
                batch.Add(new RedisKey(key));
                if (batch.Count != batchSize) continue;
                DB.KeyDelete(batch.ToArray());
                batch.Clear();
            }
            if (batch.Count != 0) // final batch
                DB.KeyDelete(batch.ToArray());
        }

        public static int DeleteKeysByPattern(string pattern)
        {
            var keys = GetKeysByPattern(pattern);
            DeleteBatchOfKeys(keys);
            return keys.Count;
        }

        //public async List<T> getObjectSpecificWhere<T>(Type type, Dictionary<string, string> dict)
        //{
        //    return await type.GetMethod("Where").Invoke(type, new[] { dict });
        //}
    }
}