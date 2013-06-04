﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Tracking.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that sets the package identifier of a TrackRequest.
    /// </summary>
    public class FedExTrackingPackageIdentifierManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            TrackRequest nativeRequest = request.NativeRequest as TrackRequest;

            // The Shipment Entity
            ShipmentEntity shipmentEntity = request.ShipmentEntity;

            string trackingNumber = shipmentEntity.TrackingNumber;

            // For testing purposes, use 999999999999999
            if (InterapptiveOnly.MagicKeysDown)
            {
                trackingNumber = "999999999999999";
            }

            nativeRequest.IncludeDetailedScans = true;
            nativeRequest.IncludeDetailedScansSpecified = true;

            nativeRequest.PackageIdentifier = new TrackPackageIdentifier
            {
                Type = TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG,
                Value = trackingNumber
            };
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.ShipmentEntity == null)
            {
                throw new CarrierException("The request did not have a valid ShipmentEntity.");
            }

            // The native FedEx request type should be a TrackRequest
            TrackRequest nativeRequest = request.NativeRequest as TrackRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
