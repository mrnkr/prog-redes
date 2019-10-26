using System;
using System.Configuration;

namespace Gestion.Common
{
    public class Config
    {
        public static T GetValue<T>(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
