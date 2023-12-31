using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;

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
        IFedExShipRequest CreateShipRequest();

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
        IFedExRateRequest CreateRateRequest();

        /// <summary>
        /// Creates the track request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to retrieve tracking data.</returns>
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
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx searching drop-off location.</returns>
        IFedExGlobalShipAddressRequest CreateSearchLocationsRequest();

        /// <summary>
        /// Creates the UploadImages request.
        /// </summary>
        /// <param name="accountEntity"></param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to upload image data.</returns>
        CarrierRequest CreateUploadImageRequest(FedExAccountEntity accountEntity);
    }
}
