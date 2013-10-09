﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will add the appropriate special
    /// shipment type attributes to the IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExShipmentSpecialServiceTypeManipulator : FedExShippingRequestManipulatorBase
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipmentSpecialServiceTypeManipulator" /> class.
        /// </summary>
        public FedExShipmentSpecialServiceTypeManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipmentSpecialServiceTypeManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExShipmentSpecialServiceTypeManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            // Use the ship date of the shipment entity to determine the ship time stamp; not looking at the 
            // ShipTimestamp property of the request here, because there's no guarantee it's been set
            DateTime shipTimestamp = new DateTime(request.ShipmentEntity.ShipDate.Ticks, DateTimeKind.Local);

            // Since we'll be assigning this list back to the native request, create a list of the existing 
            // special service types that are on the request already so we don't overwrite anything
            List<ShipmentSpecialServiceType> specialServiceTypes = new List<ShipmentSpecialServiceType>();
            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes != null)
            {
                specialServiceTypes.AddRange(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            }

            if (shipTimestamp.Date != DateTime.Today)
            {
                // This is a future delivery
                specialServiceTypes.Add(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT);
            }

            if (shipTimestamp.DayOfWeek == DayOfWeek.Saturday)
            {
                // This will be a Saturday pickup
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_PICKUP);
            }

            // Pull out the FedEx shipment info from the shipment entity and check if they want Saturday 
            // delivery and whether it could be delivered on a Saturday
            FedExShipmentEntity fedExShipmentEntity = request.ShipmentEntity.FedEx;
            if (fedExShipmentEntity.SaturdayDelivery && FedExUtility.CanDeliverOnSaturday((FedExServiceType)fedExShipmentEntity.Service, shipTimestamp))
            {
                // Saturday delivery is available 
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_DELIVERY);
            }

            // Assign the updated special service types list back to the request
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();
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
                // We'll be manipulating the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // We'll be accessing/manipulating the special services, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                // We'll be accessing/manipulating the special service types, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }
        }
    }
}
