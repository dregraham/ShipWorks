using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Response data from a FIMS ship request.
    /// </summary>
    public interface IFimsShipResponse
    {
        /// <summary>
        /// ParcelID from FIMS response
        /// </summary>
        string ParcelID { get; }

        /// <summary>
        /// ResponseCode from FIMS response
        /// </summary>
        string ResponseCode { get; }

        /// <summary>
        /// List of errors from FIMS response
        /// </summary>
        List<string> Errors { get; }

        /// <summary>
        /// The byte array data of the label as a PDF
        /// </summary>
        byte[] LabelPdfData { get; set; }
    }
}