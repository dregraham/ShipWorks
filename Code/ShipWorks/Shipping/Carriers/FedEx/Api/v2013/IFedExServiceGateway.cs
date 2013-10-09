using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Ship;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.PackageMovement;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013
{
    /// <summary>
    /// An interface that encapsulates the communication with FedEx.
    /// </summary>
    public interface IFedExServiceGateway
    {
        /// <summary>
        /// Intended to interact with the FedEx API to process a shipment.
        /// </summary>
        /// <param name="processShipmentRequest">The process shipment request.</param>
        /// <returns>The ProcessShipmentReply recevied from FedEx.</returns>
        IFedExNativeShipmentReply Ship(IFedExNativeShipmentRequest nativeShipmentRequest);

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
        SearchLocationsReply GlobalShipAddressInquiry(SearchLocationsRequest searchLocationsRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for performing an end of day ground close.
        /// </summary>
        /// <param name="groundCloseRequest">The ground close request.</param>
        /// <returns>The GroundCloseReply recevied from FedEx.</returns>
        GroundCloseReply Close(GroundCloseRequest groundCloseRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for performing an end of day SmartPost close.
        /// </summary>
        /// <param name="smartPostCloseRequest">The smart post close request.</param>
        /// <returns>The SmartPostCloseRequest recevied from FedEx.</returns>
        SmartPostCloseReply Close(SmartPostCloseRequest smartPostCloseRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for performing a shipment void.
        /// </summary>
        /// <param name="deleteShipmentRequest">The delete shipment request.</param>
        /// <returns>The ShipmentReply recevied from FedEx.</returns>
        ShipmentReply Void(DeleteShipmentRequest deleteShipmentRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for registering a CSP user.
        /// </summary>
        /// <param name="registerRequest">The register request.</param>
        /// <returns>The RegisterWebCspUserReply recevied from FedEx.</returns>
        RegisterWebCspUserReply RegisterCspUser(RegisterWebCspUserRequest registerRequest);

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
        /// <returns>The RateReply received from FedEx.</returns>
        RateReply GetRates(RateRequest rateRequest);

        /// <summary>
        /// Intended to interact with the FedEx API for tracking a shipment.
        /// </summary>
        /// <param name="trackRequest">The track shipment request.</param>
        /// <returns>The TrackReply recevied from FedEx.</returns>
        TrackReply Track(TrackRequest trackRequest);
    }
}
