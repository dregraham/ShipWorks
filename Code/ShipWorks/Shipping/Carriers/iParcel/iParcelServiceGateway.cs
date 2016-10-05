using System.Data;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Net.Authentication;
using ShipWorks.Shipping.Carriers.iParcel.Net.Ship;
using ShipWorks.Shipping.Carriers.iParcel.Net.Track;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Implementation of the IiParcelServiceGateway that dispatches requests to the i-parcel web services.
    /// </summary>
    [Component]
    public class iParcelServiceGateway : IiParcelServiceGateway
    {
        /// <summary>
        /// Intended to serve as a means to determine if a set of credentials are recognized
        /// by the i-parcel web service.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>
        ///   <c>true</c> if the specified credentials are valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidUser(iParcelCredentials credentials)
        {
            bool isValid = true;

            iParcelAuthenticationRequest authenticationRequest = new iParcelAuthenticationRequest(credentials);
            DataSet response = authenticationRequest.Submit();

            if (response.Tables.Contains("ErrorInfo"))
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Creates ship request and submits it to i-parcel
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A DataSet containing the label image data and tracking information.</returns>
        public DataSet SubmitShipment(iParcelCredentials credentials, ShipmentEntity shipment)
        {
            iParcelShipRequest request = new iParcelShipRequest(credentials, shipment);
            return request.Submit();
        }

        /// <summary>
        /// Gets the shipping rates from i-parcel.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A DataSet containing the rate results for the given shipment.</returns>
        public DataSet GetRates(iParcelCredentials credentials, ShipmentEntity shipment)
        {
            iParcelRateRequest request = new iParcelRateRequest(credentials, shipment);
            return request.Submit();
        }

        /// <summary>
        /// Gets tracking information from i-Parcel for a shipment
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A DataSet containing tracking events.</returns>
        public DataSet TrackShipment(iParcelCredentials credentials, ShipmentEntity shipment)
        {
            // Just use the first package to track the shipment to be consistent with the
            // other carrier implementations
            iParcelTrackRequest request = new iParcelTrackRequest(credentials, shipment.IParcel.Packages[0]);
            return request.Submit();
        }
    }
}
