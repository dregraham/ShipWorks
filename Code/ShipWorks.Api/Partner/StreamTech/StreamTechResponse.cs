using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Api.Partner.StreamTech
{
    /// <summary>
    /// Response data to return to StreamTech
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class StreamTechResponse
    {
        /// <summary>
        /// The response data 
        /// </summary>
        [JsonProperty("response")]
        public ResponseData Response { get; set; }
    }

    /// <summary>
    /// The data within the StreamTechResponse
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ResponseData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseData()
        {
            ErrorCode = 0;
        }

        /// <summary>
        /// Signature Value = “STSS”
        /// </summary>
        [JsonProperty("signature")]
        public string Signature => "STSS";

        /// <summary>
        /// Message Number Number that uniquely identifies a request / response pair
        /// </summary>
        [JsonProperty("msg_no")]
        public string MessageNumber { get; set; }

        /// <summary>
        /// Package ID Barcode(PIB)1 One or more barcodes delimited by pipes(i.e.LPN, etc.)
        /// </summary>
        [JsonProperty("package_id_barcode")]
        public string PackageIDBarcode { get; set; }

        /// <summary>
        /// Verify Barcode(s) Relevant carton identification barcode - used for verification.Typically
        /// will be the tracking number, but not necessarily.
        /// </summary>
        [JsonProperty("verify_barcode")]
        public string VerifyBarcodes { get; set; }

        /// <summary>
        /// Carrier code for systems with sortation.Can be zero if no sorter or if
        /// Carrier code is used for sortation.
        /// </summary>
        [JsonProperty("carrier")]
        public string CarrierCode { get; set; }

        /// <summary>
        /// Sort Code Sort code for systems with sortation.Can be zero if no sorter or if
        /// Carrier code is used for sortation.
        /// </summary>
        [JsonProperty("sort_code")]
        public string SortCode => "0";

        /// <summary>
        /// Quality control – Values are 0 or 1, If QC equals 1, then the carton will
        /// be diverted to a QC zone for manual remediation. A QC value of 0
        /// means the carton will continue without error. This is an option on the
        /// Sprinter.If there is no divert, then QC should always be 0.
        /// </summary>
        [JsonProperty("qc")]
        public string QualityControl => "0";

        /// <summary>
        /// Bypass Values are 0 or 1, If Bypass equals 1, then the carton will not have a
        /// shipping label printed and applied.
        /// </summary>
        [JsonProperty("bypass")]
        public string Bypass => "0";

        /// <summary>
        /// Expected Weight The expected weight of the carton. Printed on error label if the check
        /// weigh flag is set in the Sprinter configuration.
        /// </summary>
        [JsonProperty("expected_weight")]
        public double ExpectedWeight { get; set; }

        /// <summary>
        /// Minimum Weight If the check weigh flag is set in the Sprinter configuration then the carton
        /// must be greater than or equal to the Minimum Weight.
        /// </summary>
        [JsonProperty("minimum_weight")]
        public double MinimumWeight { get; set; }

        /// <summary>
        /// Maximum Weight If the check weigh flag is set in the Sprinter configuration then the carton
        /// must be less than or equal to the Maximum Weight.
        /// </summary>
        [JsonProperty("maximum_weight")]
        public double MaximumWeight { get; set; }

        /// <summary>
        /// Label Type
        /// </summary>
        [JsonProperty("label_type")]
        public string LabelType => "ZPL";

        /// <summary>
        /// Label Encoding Values(NONE, BASE64) or Blank, Default = NONE
        /// </summary>
        [JsonProperty("label_encoding")]
        public string LabelEncoding => "BASE64";

        /// <summary>
        /// Label Destination Charset Values(NONE, UTF8) or Blank, Default = NONE
        /// </summary>
        [JsonProperty("label_charset")]
        public string LabelDestinationCharset => "UTF8";

        /// <summary>
        /// Variable length label
        /// </summary>
        [JsonProperty("print_label")]
        public string ZplLabel { get; set; }

        /// <summary>
        /// The error code
        /// </summary>
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}
