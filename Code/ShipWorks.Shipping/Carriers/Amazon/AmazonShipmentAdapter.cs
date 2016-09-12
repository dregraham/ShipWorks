﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Adapter for other specific shipment information
    /// </summary>
    public class AmazonShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private AmazonShipmentAdapter(AmazonShipmentAdapter adapterToCopy) : base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            IStoreManager storeManager) : base(shipment, shipmentTypeManager, null, storeManager)
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
        public override bool SupportsAccounts => false;

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public override bool SupportsPackageTypes => false;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType { get; set; } = 0;

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        /// <returns></returns>
        public override ICarrierShipmentAdapter Clone() => new AmazonShipmentAdapter(this);

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

        /// <summary>
        /// Amazon does not have customs items
        /// </summary>
        public override IEnumerable<IShipmentCustomsItemAdapter> GetCustomsItemAdapters() =>
            Enumerable.Empty<IShipmentCustomsItemAdapter>();
    }
}