using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Api.Partner.StreamTech
{
    /// <summary>
    /// Request data from the StreamTech system
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class StreamTechRequest
    {
        /// <summary>
        /// The request
        /// </summary>
        [JsonProperty("request")]
        public RequestData Request { get; set; }
    }

    /// <summary>
    /// Data within a StreamTechRequest
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RequestData
    {
        /// <summary>
        /// Signature Value = “STSS”
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// Message Number Number that uniquely identifies a request / response pair
        /// </summary>
        [JsonProperty("msg_no")]
        public string MessageNumber { get; set; }

        /// <summary>
        /// Weight In pounds / xx.x characters
        /// </summary>
        [JsonProperty("weight")]
        public double Weight { get; set; }

        /// <summary>
        /// Length Inches / xx.x characters
        /// </summary>
        [JsonProperty("length")]
        public double Length { get; set; }

        /// <summary>
        /// Width Inches / xx.x characters
        /// </summary>
        [JsonProperty("width")]
        public double Width { get; set; }

        /// <summary>
        /// Height Inches / xx.x characters
        /// </summary>
        [JsonProperty("height")]
        public double Height { get; set; }

        /// <summary>
        /// Package Type Numeric Value – Custom Field per project
        /// </summary>
        [JsonProperty("package_type")]
        public string PackageType { get; set; }

        /// <summary>
        /// System ID For multiple system(i.e.sorter 1, sorter 2, etc.)
        /// </summary>
        [JsonProperty("system_id")]
        public string SystemID { get; set; }

        /// <summary>
        /// Package ID Barcode(PIB)1 One or more barcodes delimited by pipes(i.e.LPN, etc.)
        /// </summary>
        [JsonProperty("package_id_barcode")]
        public string PackageIDBarcode { get; set; }
    }
}
