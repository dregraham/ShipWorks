using System.Collections.Generic;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// An interface for creating requests that communicate with a shipping carrier's API.
    /// </summary>
    public interface IFedExRequestFactory
    {
        /// <summary>
        /// Creates the carrier-specific request to ship an order/create a label.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting requests to a carrier API to 
        /// ship an order/create a label.</returns>
        CarrierRequest CreateShipRequest(ShipmentEntity shipmentEntity);

        /// <summary>
        /// Creates the package movement request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="account">The account.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx for package movements.</returns>
        CarrierRequest CreatePackageMovementRequest(ShipmentEntity shipmentEntity, FedExAccountEntity account);

        /// <summary>
        /// Creates the version capture request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="accountLocationId">The account location ID.</param>
        /// <param name="account">The account.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to do the version capture.</returns>
        CarrierRequest CreateVersionCaptureRequest(ShipmentEntity shipmentEntity, string accountLocationId, FedExAccountEntity account);

        /// <summary>
        /// Creates the ground close request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to do the end of day close.</returns>
        CarrierRequest CreateGroundCloseRequest(FedExAccountEntity accountEntity);

        /// <summary>
        /// Creates the smart post close request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to do the end of day close.</returns>
        CarrierRequest CreateSmartPostCloseRequest(FedExAccountEntity accountEntity);

        /// <summary>
        /// Creates the void request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to void a shipment.</returns>
        CarrierRequest CreateVoidRequest(FedExAccountEntity accountEntity, ShipmentEntity shipmentEntity);

        /// <summary>
        /// Creates the register CSP user request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to register a CSP user.</returns>
        CarrierRequest CreateRegisterCspUserRequest(FedExAccountEntity accountEntity);

        /// <summary>
        /// Creates the subscription request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to subscribe a shipper to use the FedEx API.</returns>
        CarrierRequest CreateSubscriptionRequest(FedExAccountEntity accountEntity);

        /// <summary>
        /// Creates the rate request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="specializedManipulators">Any specialized manipulators that should be added to the request in addition
        /// to the standard/basic manipulators of the rate request.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx for obtaining shipping rates.</returns>
        CarrierRequest CreateRateRequest(ShipmentEntity shipmentEntity, IEnumerable<ICarrierRequestManipulator> specializedManipulators);

        /// <summary>
        /// Creates the track request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to retrive tracking data.</returns>
        CarrierRequest CreateTrackRequest(FedExAccountEntity accountEntity, ShipmentEntity shipmentEntity);

        /// <summary>
        /// Creates the certificate request.
        /// </summary>
        /// <param name="certificateInspector">The certificate inspector.</param>
        /// <returns>An instance of an ICertificateRequest that can be used to check the security level
        /// of a host's certificate.</returns>
        ICertificateRequest CreateCertificateRequest(ICertificateInspector certificateInspector);

        /// <summary>
        /// Creates the Search Location request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx searching dropoff location.</returns>
        FedExGlobalShipAddressRequest CreateSearchLocationsRequest(ShipmentEntity shipment, FedExAccountEntity account);
    }
}
