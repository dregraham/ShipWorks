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
        /// <summary>
        /// Constructor
        /// </summary>
        public FimsShipResponse(string parcelID, byte[] labelData, string labelFormat, string trackingNumber)
        {
            ParcelID = parcelID;
            LabelData = labelData;
            LabelFormat = labelFormat;
            TrackingNumber = trackingNumber;
        }

        /// <summary>
        /// ParcelID from FIMS response
        /// </summary>
        public string ParcelID { get; }

        /// <summary>
        /// The byte array data of the label
        /// </summary>
        public byte[] LabelData { get; }

        /// <summary>
        /// Label Format 
        /// </summary>
        /// <remarks>
        /// Z = ZPL, I = Image
        /// </remarks>
        public string LabelFormat { get; }

        /// <summary>
        /// Tracking Number allowed on FedEx website
        /// </summary>
        public string TrackingNumber { get; }
    }
}
