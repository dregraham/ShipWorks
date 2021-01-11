using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ShipWorks.Data.Serialization
{
    /// <summary>
    /// JSON serializer settings for LLBLgen entities
    /// </summary>
    public class EntityJsonSerializerSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityJsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            ContractResolver = new DefaultContractResolver()
            {
                IgnoreSerializableAttribute = true,
                IgnoreSerializableInterface = true
            };
        }
    }
}
