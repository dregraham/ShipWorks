using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// An interface that encapsulates the communication with FedEx.
    /// </summary>
    public interface IFedExServiceGateway
    {
        /// <summary>
        /// Intended to interact with the FedEx API to process a shipment.
        /// </summary>
        /// <returns>The ProcessShipmentReply received from FedEx.</returns>
        TelemetricResult<GenericResult<ProcessShipmentReply>> Ship(ProcessShipmentRequest nativeShipmentRequest);

        /// <summary>
        /// Intended to interact with the FedEx API to get locationID
        /// </summary>
        /// <returns>The PostalCodeInquiryReply received from FedEx</returns>
        PostalCodeInquiryReply PostalCodeInquiry(PostalCodeInquiryRequest postalCodeInquiryRequest);

        /// <summary>
        /// Makes Version Capture call to FedEx
        /// </summary>
        /// <returns>The VersionCaptureReply received from FedEx</returns>
        VersionCaptureReply VersionCapture(VersionCaptureRequest versionCaptureRequest);

        /// <summary>
        /// Makes GlobalShipAddressInquiry call to FedEx
        /// </summary>
        /// <returns>The SearchLocationsReply received from FedEx</returns>
        GenericResult<SearchLocationsReply> GlobalShipAddressInquiry(SearchLocationsRequest searchLocationsRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for performing an end of day ground close.
        /// </summary>
        /// <param name="groundCloseRequest">The ground close request.</param>
        /// <returns>The GroundCloseReply received from FedEx.</returns>
        GroundCloseReply Close(GroundCloseRequest groundCloseRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for performing an end of day SmartPost close.
        /// </summary>
        /// <param name="smartPostCloseRequest">The smart post close request.</param>
        /// <returns>The SmartPostCloseRequest received from FedEx.</returns>
        SmartPostCloseReply Close(SmartPostCloseRequest smartPostCloseRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for performing a shipment void.
        /// </summary>
        /// <param name="deleteShipmentRequest">The delete shipment request.</param>
        /// <returns>The ShipmentReply received from FedEx.</returns>
        ShipmentReply Void(DeleteShipmentRequest deleteShipmentRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for registering a CSP user.
        /// </summary>
        /// <param name="registerRequest">The register request.</param>
        /// <returns>The RegisterWebCspUserReply received from FedEx.</returns>
        RegisterWebUserReply RegisterCspUser(RegisterWebUserRequest registerRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for subscribing a shipper to use the FedEx API.
        /// </summary>
        /// <param name="subscriptionRequest">The subscription request.</param>
        /// <returns>The SubscriptionReply received from FedEx.</returns>
        SubscriptionReply SubscribeShipper(SubscriptionRequest subscriptionRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for obtaining shipping rates.
        /// </summary>
        /// <param name="rateRequest">The rate request.</param>
        /// <param name="shipmentEntity"></param>
        /// <returns>The RateReply received from FedEx.</returns>
        RateReply GetRates(RateRequest rateRequest, IShipmentEntity shipmentEntity);

        /// <summary>
        /// Intended to interact with the FedEx API for tracking a shipment.
        /// </summary>
        /// <param name="trackRequest">The track shipment request.</param>
        /// <returns>The TrackReply received from FedEx.</returns>
        TrackReply Track(TrackRequest trackRequest);
    }
}
