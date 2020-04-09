using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Api.ThirdPartyIntegrations.StreamTech
{
    /// <summary>
    /// Response data to return to StreamTech
    /// </summary>
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
    public class ResponseData
    {
        /// <summary>
        /// Signature Value = “STSS”
        /// </summary>
        public string Signature => "STSS";

        /// <summary>
        /// Message Number Number that uniquely identifies a request / response pair
        /// </summary>
        public string MessageNumber { get; set; }

        /// <summary>
        /// Package ID Barcode(PIB)1 One or more barcodes delimited by pipes(i.e.LPN, etc.)
        /// </summary>
        public string PackageIDBarcode { get; set; }

        /// <summary>
        /// Verify Barcode(s) Relevant carton identification barcode - used for verification.Typically
        /// will be the tracking number, but not necessarily.
        /// </summary>
        public string VerifyBarcodes { get; set; }

        /// <summary>
        /// Carrier code for systems with sortation.Can be zero if no sorter or if
        /// Carrier code is used for sortation.
        /// </summary>
        public string CarrierCode { get; set; }

        /// <summary>
        /// Sort Code Sort code for systems with sortation.Can be zero if no sorter or if
        /// Carrier code is used for sortation.
        /// </summary>
        public string SortCode => "0";

        /// <summary>
        /// Quality control – Values are 0 or 1, If QC equals 1, then the carton will
        /// be diverted to a QC zone for manual remediation. A QC value of 0
        /// means the carton will continue without error. This is an option on the
        /// Sprinter.If there is no divert, then QC should always be 0.
        /// </summary>
        public string QualityControl => "0";

        /// <summary>
        /// Bypass Values are 0 or 1, If Bypass equals 1, then the carton will not have a
        /// shipping label printed and applied.
        /// </summary>
        public string Bypass => "0";

        /// <summary>
        /// Expected Weight The expected weight of the carton. Printed on error label if the check
        /// weigh flag is set in the Sprinter configuration.
        /// </summary>
        public double ExpectedWeight { get; set; }

        /// <summary>
        /// Minimum Weight If the check weigh flag is set in the Sprinter configuration then the carton
        /// must be greater than or equal to the Minimum Weight.
        /// </summary>
        public double MinimumWeight { get; set; }

        /// <summary>
        /// Maximum Weight If the check weigh flag is set in the Sprinter configuration then the carton
        /// must be less than or equal to the Maximum Weight.
        /// </summary>
        public double MaximumWeight { get; set; }

        /// <summary>
        /// Label Type Values (ZPL, XML:<ZPL Template Name>) or Blank, Default = ZPL
        /// </summary>
        public string LabelType => "ZPL";

        /// <summary>
        /// Label Encoding Values(NONE, BASE64) or Blank, Default = NONE
        /// </summary>
        public string LabelEncoding => "BASE64";

        /// <summary>
        /// Label Destination Charset Values(NONE, UTF8) or Blank, Default = NONE
        /// </summary>
        public string LabelDestinationCharset => "NONE";

        /// <summary>
        /// Variable length label delimited by ^XA<DATA>^XZ 
        /// </summary>
        public string ZplLabel { get; set; }

        /// <summary>
        /// error description if no Zpl Label
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Error Code Error code returned
        /// </summary>
        public string ErrorCode { get; set; }
    }
}
