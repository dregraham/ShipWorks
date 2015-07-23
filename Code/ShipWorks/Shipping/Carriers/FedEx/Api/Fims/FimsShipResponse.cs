using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Response data from a FIMS ship request.
    /// </summary>
    public class FimsShipResponse : IFimsShipResponse
    {
        private readonly List<string> errors = new List<string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public FimsShipResponse(string parcelID, string responseCode)
        {
            ParcelID = parcelID;
            ResponseCode = responseCode;
        }

        /// <summary>
        /// ParcelID from FIMS response
        /// </summary>
        public string ParcelID { get; private set; }

        /// <summary>
        /// ResponseCode from FIMS response
        /// </summary>
        public string ResponseCode { get; private set; }

        /// <summary>
        /// List of errors from FIMS response
        /// </summary>
        public List<string> Errors
        {
            get { return errors; }
        }

        /// <summary>
        /// The byte array data of the label as a PDF
        /// </summary>
        public byte[] LabelPdfData { get; set; }
    }
}
