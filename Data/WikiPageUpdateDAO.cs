





// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//      Generated: Sun, 15 Jan 2023 23:38:58 GMT
// </auto-generated>
// ------------------------------------------------------------------------------
using WikiPageApp.Data;
using WikiPageApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
namespace WikiPageApp.Data
{
    public class WikiPageUpdateDAO
    {
        private readonly WikiPageUpdateDataContext _context;

        public WikiPageUpdateDAO()
        {
            var objName = nameof(WikiPageUpdate);
            var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName);
            _context = new WikiPageUpdateDataContext(connectionString, EFProvider);
        }

        public async Task<List<WikiPageUpdate>> GetAll()
        {
            //_context.WikiPageUpdate.
            return _context.WikiPageUpdate.Where(a => a.WikiPageUpdateIsSoftDeleted == false).ToList();
        }

        public async Task<List<WikiPageUpdate>> Where(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("WikiPageUpdateIsSoftDeleted")) dict.Add("WikiPageUpdateIsSoftDeleted", "false");

            var keysAndValues = dict.Select(keyValuePair => $"{keyValuePair.Key} == \"{keyValuePair.Value}\"").ToList();

            return await _context.WikiPageUpdate
                .Where(string.Join(" && ", keysAndValues))
                .ToDynamicListAsync<WikiPageUpdate>();
        }

        public async Task<List<WikiPageUpdate>> Where(string whereClause, bool ignoreDeleted = true)
        {
            if(ignoreDeleted && !whereClause.Contains("WikiPageUpdateIsSoftDeleted")) whereClause += " && WikiPageUpdateIsSoftDeleted = false";

            return await _context.WikiPageUpdate
                           .Where(whereClause)
                           .ToDynamicListAsync<WikiPageUpdate>();
        }
        public async Task<WikiPageUpdate> Save(WikiPageUpdate wikiPageUpdate)
        {
            var existing = await _context.WikiPageUpdate.FindAsync(wikiPageUpdate.WikiPageUpdateID);

            if (existing == null) _context.WikiPageUpdate.Add(wikiPageUpdate);
            else
            {
                _context.WikiPageUpdate.Remove(existing);
                _context.WikiPageUpdate.Add(wikiPageUpdate);
            }

            await _context.SaveChangesAsync();
            return wikiPageUpdate;
        }


        

        public async Task<WikiPageUpdate> GetCached(Dictionary<string, string> dict)
        {
            return await GetCachedFromRedis<WikiPageUpdate>(nameof(WikiPageUpdate), dict, true);
        }

        public async Task<T> GetCachedProperty<T>(string propertyName, Dictionary<string, string> dict)
        {
            return await GetCachedFromRedis<T>(propertyName, dict);
        }
        public async Task<T> GetCachedFromRedis<T>(string propertyName, Dictionary<string, string> dict, bool wholeObject = false)
        {
            var redisKey = new StringBuilder("Get" + propertyName + "From");
            redisKey.AppendJoin("_", dict.Keys.ToList());
            redisKey.Append("=" + string.Join("::", dict.Values.ToList()));

            var redisStringValue = RedisUtils.DB.StringGet(redisKey.ToString());
            var returnTypedValue = redisStringValue.IsNull ? await GetValueFromDB() : RedisStringValueConverter();

            if (wholeObject) RedisUtils.SaveObjectToRedis(redisKey.ToString(), returnTypedValue!, TimeSpan.FromDays(30));
            else RedisUtils.SaveStringToRedis(redisKey.ToString(), returnTypedValue!.ToString()!, TimeSpan.FromDays(30));

            return (T)returnTypedValue!;

            async Task<T> GetValueFromDB()
            {
                var obj = (await Where(dict)).FirstOrDefault(); // <== this is what bind this to the generator per object !!
                if (obj == null) throw new Exception(redisKey + " returns no records ");
                if (wholeObject) return (T)Convert.ChangeType(obj, typeof(T));

                var dbValue = (T)obj.GetType().GetProperty(propertyName)?.GetValue(obj, null)!;
                if (dbValue == null) throw new Exception(redisKey + " is null or not found ");
                return (T)dbValue;
            }

            T RedisStringValueConverter()
            {
                return wholeObject
                    ? JsonConvert.DeserializeObject<T>(redisStringValue)
                    : (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(redisStringValue.ToString())!;
            }
        }

        void CacheSaveString(List<string> keys, string value, WikiPageUpdate wikiPageUpdate, TimeSpan expiry)
        {
            RedisUtils.SaveObjectToRedis(keys, value, expiry);
        }

        // key = AppName
        // value = AppID 

        void CacheSaveObject(List<string> keys, Dictionary<string, string> values, WikiPageUpdate wikiPageUpdate, TimeSpan expiry)
        {
            var valueDict = new Dictionary<string, object>();
            // prop nickname, propname 
            // name, ApplicationName50 
            foreach (var value in values)
            {
                //valueDict.Add(value.Key, "reflect the value of the property");
                valueDict.Add(value.Key, GetPropertyValue(wikiPageUpdate, value.Key));
            }
            RedisUtils.SaveObjectToRedis(keys, valueDict, expiry);
        }
        private static object GetPropertyValue(object source, string propertyName)
        {
            PropertyInfo property = source.GetType().GetProperty(propertyName);
            return property.GetValue(source, null);
        }
        //dynamic object 
        string CacheGet(List<string> keys)
        {
            return RedisUtils.GetStringFromRedis(keys);
        }
        /*
        //Unique Property Code
        public async Task<Guid> GetCachedMainEntityIDFromPropertyNVarchar50(string value)
        {
            return await GetCachedProperty<Guid>("MainEntityID",
                  new Dictionary<string, string>() { { "PropertyNVarchar50", value } });
        }
        public MainEntity? GetByPropertyNVarchar50(string propertyNVarchar50)
        {
            var app = _context.MainEntity
                .FirstOrDefault(a => a.PropertyNVarchar50 == propertyNVarchar50 && a.MainEntityIsSoftDeleted == false);
            return app;
        }
        */
           
    }
}