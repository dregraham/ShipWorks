using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Manage label through Amazon
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.Amazon)]
    public class AmazonLabelService : ILabelService
    {
        private readonly IAmazonCreateShipmentRequest createShipmentRequest;
        private readonly IAmazonCancelShipmentRequest cancelShipmentRequest;
        private readonly Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IAmazonCreateShipmentRequest createShipmentRequest,
            IAmazonCancelShipmentRequest cancelShipmentRequest,
            Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData> createDownloadedLabelData)
        {
            this.createShipmentRequest = createShipmentRequest;
            this.cancelShipmentRequest = cancelShipmentRequest;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Create the label
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            
            AmazonShipment labelResponse = createShipmentRequest.Submit(shipment);

            return createDownloadedLabelData(shipment, labelResponse);
        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            cancelShipmentRequest.Submit(shipment);
        }
    }
}
