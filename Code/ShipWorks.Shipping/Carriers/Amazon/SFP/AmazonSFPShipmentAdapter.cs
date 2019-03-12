﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Adapter for other specific shipment information
    /// </summary>
    public class AmazonSFPShipmentAdapter : CarrierShipmentAdapterBase
    {
        private readonly IAmazonSFPServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Copy constructor
        /// </summary>
        private AmazonSFPShipmentAdapter(AmazonSFPShipmentAdapter adapterToCopy, IAmazonSFPServiceTypeRepository serviceTypeRepository) : base(adapterToCopy)
        {
            this.serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            IStoreManager storeManager, IAmazonSFPServiceTypeRepository serviceTypeRepository) : base(shipment, shipmentTypeManager, null, storeManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon, nameof(shipment.Amazon));
            this.serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// Id of the other account associated with this shipment
        /// </summary>
        [SuppressMessage("MS", "RECS0029:\"value\" parameters should be used", Justification = "Amazon shipment types don't have accounts")]
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
        public override int ServiceType
        {
            get
            {
                return serviceTypeRepository.Get()
                    .FirstOrDefault(s => s.ApiValue == Shipment.Amazon.ShippingServiceID).AmazonServiceTypeID;
            }
            set
            {
                Shipment.Amazon.ShippingServiceID = serviceTypeRepository.Get()
                    .FirstOrDefault(s => s.AmazonServiceTypeID == value).ApiValue;
            }
        }

        /// <summary>
        /// Get the service type name
        /// </summary>
        public override string ServiceTypeName => Shipment.Amazon.ShippingServiceName;

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        /// <returns></returns>
        public override ICarrierShipmentAdapter Clone() => new AmazonSFPShipmentAdapter(this, serviceTypeRepository);

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
                Shipment.Amazon.CarrierName = rateTag.CarrierName ?? string.Empty;
                ServiceType = rateTag.ServiceTypeID;
            }
        }

        /// <summary>
        /// Converts tag to an AmazonRateTag and returns the ServiceTypeID
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            AmazonRateTag rateTag = tag as AmazonRateTag;

            return rateTag?.ServiceTypeID;
        }

        /// <summary>
        /// Is the rate the same as the shipments service
        /// </summary>
        public override bool DoesRateMatchSelectedService(RateResult rate)
        {
            AmazonRateTag rateTag = rate.Tag as AmazonRateTag;

            return rateTag?.ShippingServiceId == Shipment.Amazon.ShippingServiceID;
        }

        /// <summary>
        /// Amazon does not have customs items
        /// </summary>
        public override IEnumerable<IShipmentCustomsItemAdapter> GetCustomsItemAdapters() =>
            Enumerable.Empty<IShipmentCustomsItemAdapter>();
    }
}
