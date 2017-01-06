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
        /// The byte array data of the label
        /// </summary>
        byte[] LabelData { get; }

        /// <summary>
        /// Label Format 
        /// </summary>
        /// <remarks>
        /// Z = ZPL, I = Image
        /// </remarks>
        string LabelFormat { get; }

        /// <summary>
        /// Tracking Number allowed on FedEx website
        /// </summary>
        string TrackingNumber { get; }
    }
}