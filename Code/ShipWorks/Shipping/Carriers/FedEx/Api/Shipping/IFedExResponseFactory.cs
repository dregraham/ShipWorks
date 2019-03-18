﻿using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping
{
    /// <summary>
    /// An implementation of the ICarrierResponseFactory for FedEx.
    /// </summary>
    public interface IFedExResponseFactory : ICarrierResponseFactory
    {
        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the end of day ground close.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a ground close request.</returns>
        ICarrierResponse CreateGroundCloseResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the end of day SmartPost close.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a SmartPost close request.</returns>
        ICarrierResponse CreateSmartPostCloseResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the void.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        ICarrierResponse CreateVoidResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing tracking.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        ICarrierResponse CreateTrackResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the uploading of images.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>ICarrierResponse representing the response of an UploadImages request.</returns>
        ICarrierResponse CreateUploadImagesResponse(object nativeResponse, CarrierRequest request);
    }
}