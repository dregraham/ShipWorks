﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators
{
    public class FedExRateTypeManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateTypeManipulator" /> class.
        /// </summary>
        public FedExRateTypeManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateTypeManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExRateTypeManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateTypeManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRateTypeManipulator(ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            // Use the repository to see what type of rates we should be using
            RateRequestType[] rateTypes = new RateRequestType[] { SettingsRepository.UseListRates ? RateRequestType.LIST : RateRequestType.ACCOUNT };
            nativeRequest.RequestedShipment.RateRequestTypes = rateTypes;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }
    }
}
