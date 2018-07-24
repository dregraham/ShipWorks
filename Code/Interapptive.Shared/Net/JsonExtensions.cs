using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Extension methods for Json.Net objects
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Extension method to get a value, and return a default value if not present
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="input">JToken to query</param>
        /// <param name="key">The field to try and get</param>
        /// <param name="defaultValue">If field is not found in input, return this value.  This defaults to the T type's default.</param>
        /// <returns></returns>
        public static T GetValue<T>(this JToken input, string key, T defaultValue = default(T))
        {
            if (input == null)
            {
                throw new JsonException($"JObject for {key} was null.");
            }

            JToken selection = input.SelectToken(key);
            if (selection != null && selection.Type != JTokenType.Null)
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    string tmp = selection.ToString();
                    return (T) converter.ConvertFromString(tmp);
                }
            }

            return defaultValue;
        }
    }
}
