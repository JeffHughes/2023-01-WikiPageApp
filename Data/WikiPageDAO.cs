





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
    public class WikiPageDAO
    {
        private readonly WikiPageDataContext _context;

        public WikiPageDAO()
        {
            var objName = nameof(WikiPage);
            var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName);
            _context = new WikiPageDataContext(connectionString, EFProvider);
        }

        public async Task<List<WikiPage>> GetAll()
        {
            //_context.WikiPage.
            return _context.WikiPage.Where(a => a.WikiPageIsSoftDeleted == false).ToList();
        }

        public async Task<List<WikiPage>> Where(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("WikiPageIsSoftDeleted")) dict.Add("WikiPageIsSoftDeleted", "false");

            var keysAndValues = dict.Select(keyValuePair => $"{keyValuePair.Key} == \"{keyValuePair.Value}\"").ToList();

            return await _context.WikiPage
                .Where(string.Join(" && ", keysAndValues))
                .ToDynamicListAsync<WikiPage>();
        }

        public async Task<List<WikiPage>> Where(string whereClause, bool ignoreDeleted = true)
        {
            if(ignoreDeleted && !whereClause.Contains("WikiPageIsSoftDeleted")) whereClause += " && WikiPageIsSoftDeleted = false";

            return await _context.WikiPage
                           .Where(whereClause)
                           .ToDynamicListAsync<WikiPage>();
        }
        public async Task<WikiPage> Save(WikiPage wikiPage)
        {
            var existing = await _context.WikiPage.FindAsync(wikiPage.WikiPageID);

            if (existing == null) _context.WikiPage.Add(wikiPage);
            else
            {
                _context.WikiPage.Remove(existing);
                _context.WikiPage.Add(wikiPage);
            }

            await _context.SaveChangesAsync();
            return wikiPage;
        }


        

        public async Task<WikiPage> GetCached(Dictionary<string, string> dict)
        {
            return await GetCachedFromRedis<WikiPage>(nameof(WikiPage), dict, true);
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

        void CacheSaveString(List<string> keys, string value, WikiPage wikiPage, TimeSpan expiry)
        {
            RedisUtils.SaveObjectToRedis(keys, value, expiry);
        }

        // key = AppName
        // value = AppID 

        void CacheSaveObject(List<string> keys, Dictionary<string, string> values, WikiPage wikiPage, TimeSpan expiry)
        {
            var valueDict = new Dictionary<string, object>();
            // prop nickname, propname 
            // name, ApplicationName50 
            foreach (var value in values)
            {
                //valueDict.Add(value.Key, "reflect the value of the property");
                valueDict.Add(value.Key, GetPropertyValue(wikiPage, value.Key));
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