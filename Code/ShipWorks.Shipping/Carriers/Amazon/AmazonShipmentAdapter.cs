using System;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Adapter for other specific shipment information
    /// </summary>
    public class AmazonShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager) :
            base(shipment, shipmentTypeManager, null)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon, nameof(shipment.Amazon));
        }

        /// <summary>
        /// Id of the other account associated with this shipment
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", Justification = "Amazon shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty", Justification = "Amazon shipment types don't have accounts")]
        public override long? AccountId
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public override bool SupportsAccounts
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public override bool SupportsPackageTypes => false;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType { get; set; } = 0;

        /// <summary>
        /// The shipment's customs items
        /// </summary>
        public override EntityCollection<ShipmentCustomsItemEntity> CustomsItems
        {
            get { return new EntityCollection<ShipmentCustomsItemEntity>(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Are customs allowed?
        /// </summary>
        public override bool CustomsAllowed => false;

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // Nothing to do as Amazon is only allowed to use ShipWorks insurance
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            AmazonRateTag rateTag = rate.Tag as AmazonRateTag;

            if (rateTag != null)
            {
                Shipment.Amazon.ShippingServiceName = rateTag.Description ?? string.Empty;
                Shipment.Amazon.ShippingServiceID = rateTag.ShippingServiceId ?? string.Empty;
                Shipment.Amazon.ShippingServiceOfferID = rateTag.ShippingServiceOfferId ?? string.Empty;
                Shipment.Amazon.CarrierName = rateTag.CarrierName ?? string.Empty;
            }
        }
    }
}
