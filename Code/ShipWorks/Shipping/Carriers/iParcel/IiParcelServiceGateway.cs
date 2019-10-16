using System.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public interface IiParcelServiceGateway
    {
        /// <summary>
        /// Intended to serve as a means to determine if a set of credentials are recognized
        /// by the the i-parcel web service.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>
        ///   <c>true</c> if the specified credentials are valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValidUser(iParcelCredentials credentials);

        /// <summary>
        /// Creates ship request and submits it to i-parcel
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A DataSet containing the label image data and tracking information.</returns>
        DataSet SubmitShipment(iParcelCredentials credentials, ShipmentEntity shipment, TelemetricResult<IDownloadedLabelData> telemetricResult);

        /// <summary>
        /// Gets the shipping rates from i-parcel.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A DataSet containing the rate results for the given shipment.</returns>
        DataSet GetRates(iParcelCredentials credentials, ShipmentEntity shipment);

        /// <summary>
        /// Gets tracking information from i-Parcel for a shipment
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A DataSet containing tracking events.</returns>
        DataSet TrackShipment(iParcelCredentials credentials, ShipmentEntity shipment);
    }
}
