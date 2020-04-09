﻿using System;
using Newtonsoft.Json;

namespace ShipWorks.Api.ThirdPartyIntegrations.StreamTech
{
    /// <summary>
    /// Request data from the StreamTech system
    /// </summary>
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
    public class RequestData
    {
        /// <summary>
        /// Signature Value = “STSS”
        /// </summary>
        [JsonProperty("signature")]
        public string Signature => "STSS";

        /// <summary>
        /// Message Number Number that uniquely identifies a request / response pair
        /// </summary>
        [JsonProperty("msg_no")]
        public string MessageNumber { get; }

        /// <summary>
        /// Weight In pounds / xx.x characters
        /// </summary>
        [JsonProperty("weight")]
        public double Weight { get; }

        /// <summary>
        /// Length Inches / xx.x characters
        /// </summary>
        [JsonProperty("length")]
        public double Length { get; }

        /// <summary>
        /// Width Inches / xx.x characters
        /// </summary>
        [JsonProperty("width")]
        public double Width { get; }

        /// <summary>
        /// Height Inches / xx.x characters
        /// </summary>
        [JsonProperty("height")]
        public double Height { get; }

        /// <summary>
        /// Package Type Numeric Value – Custom Field per project
        /// </summary>
        [JsonProperty("package_type")]
        public string PackageType { get; }

        /// <summary>
        /// System ID For multiple system(i.e.sorter 1, sorter 2, etc.)
        /// </summary>
        [JsonProperty("system_id")]
        public string SystemID { get; }

        /// <summary>
        /// Package ID Barcode(PIB)1 One or more barcodes delimited by pipes(i.e.LPN, etc.)
        /// </summary>
        [JsonProperty("package_id_barcode")]
        public string PackageIDBarcode { get; }
    }
}
