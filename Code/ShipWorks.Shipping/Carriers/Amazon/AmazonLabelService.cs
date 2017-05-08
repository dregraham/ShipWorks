using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
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
        private readonly IEnumerable<IAmazonLabelEnforcer> labelEnforcers;
        private readonly IAmazonCreateShipmentRequest createShipmentRequest;
        private readonly IAmazonCancelShipmentRequest cancelShipmentRequest;
        private readonly Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IEnumerable<IAmazonLabelEnforcer> labelEnforcers,
            IAmazonCreateShipmentRequest createShipmentRequest,
            IAmazonCancelShipmentRequest cancelShipmentRequest,
            Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData> createDownloadedLabelData)
        {
            this.labelEnforcers = labelEnforcers;
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

            EnforceLabelPolicies(shipment);

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

        /// <summary>
        /// Enforce label policies for Amazon
        /// </summary>
        private void EnforceLabelPolicies(ShipmentEntity shipment)
        {
            EnforcementResult result = labelEnforcers.Select(x => x.CheckRestriction(shipment))
                .FirstOrDefault(x => x != EnforcementResult.Success);

            if (result != null)
            {
                throw new AmazonShippingException(result.FailureReason);
            }
        }
    }
}
