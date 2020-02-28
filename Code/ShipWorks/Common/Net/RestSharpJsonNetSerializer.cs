using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serialization;

namespace ShipWorks.Common.Net
{
    /// <summary>
    /// Json.net serializer for RestSharp requests
    /// </summary>
    public class RestSharpJsonNetSerializer : IRestSerializer
    {
        /// <summary>
        /// Create a serializer with default settings for Hub communication
        /// </summary>
        public static IRestSerializer CreateHubDefault() => new RestSharpJsonNetSerializer(new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            },
        });

        /// <summary>
        /// Constructor
        /// </summary>
        public RestSharpJsonNetSerializer()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RestSharpJsonNetSerializer(JsonSerializerSettings settings)
        {
            this.Settings = settings;
        }

        public JsonSerializerSettings Settings { get; }

        /// <summary>
        /// Serialize the object
        /// </summary>
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj, Settings);

        /// <summary>
        /// Serialize the parameter
        /// </summary>
        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value, Settings);

        /// <summary>
        /// Deserialize the response
        /// </summary>
        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, Settings);

        /// <summary>
        /// Supported content types
        /// </summary>
        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        /// <summary>
        /// Content type
        /// </summary>
        public string ContentType { get; set; } = "application/json";

        /// <summary>
        /// Data format
        /// </summary>
        public DataFormat DataFormat { get; } = DataFormat.Json;
    }
}
