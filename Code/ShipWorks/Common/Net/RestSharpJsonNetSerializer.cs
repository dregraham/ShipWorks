using Newtonsoft.Json;
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
        /// Serialize the object
        /// </summary>
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

        /// <summary>
        /// Serialize the parameter
        /// </summary>
        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        /// <summary>
        /// Deserialize the response
        /// </summary>
        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content);

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
