using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentAdapter), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWAShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private AmazonSWAShipmentAdapter(AmazonSWAShipmentAdapter adapterToCopy) : base(adapterToCopy)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            ICustomsManager customsManager, IStoreManager storeManager) :
            base(shipment, shipmentTypeManager, customsManager, storeManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.AmazonSWA, nameof(shipment.AmazonSWA));
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.AmazonSWA.AmazonSWAAccountID; }
            set { Shipment.AmazonSWA.AmazonSWAAccountID = value.GetValueOrDefault(); }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public override bool SupportsAccounts => true;

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public override bool SupportsPackageTypes => false;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType
        {
            get { return (int) Shipment.AmazonSWA.Service; }
            set { Shipment.AmazonSWA.Service = value; }
        }

        /// <summary>
        /// Service type name
        /// </summary>
        public override string ServiceTypeName => EnumHelper.GetDescription((AmazonSWAServiceType) ServiceType);

        /// <summary>
        /// Are customs allowed?
        /// </summary>
        public override bool CustomsAllowed => false;

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            Shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Does the given rate match the service selected for the shipment
        /// </summary>
        public override bool DoesRateMatchSelectedService(RateResult rate)
        {
            if (ShipmentTypeCode != rate.ShipmentType)
            {
                return false;
            }

            AmazonSWAServiceType selectedValue = EnumHelper.GetEnumList<AmazonSWAServiceType>()
                .FirstOrDefault(f => f.ApiValue == rate.Days).Value;

            return (int) selectedValue == ServiceType;
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            AmazonSWAServiceType? service = tag as AmazonSWAServiceType?;

            return service.HasValue ? (int?) service.Value : base.GetServiceTypeAsIntFromTag(tag);
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            AmazonSWAServiceType selectedValue = EnumHelper.GetEnumList<AmazonSWAServiceType>()
                .FirstOrDefault(f => f.ApiValue == rate.Days).Value;

            Shipment.AmazonSWA.Service = (int) selectedValue;
        }

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        public override ICarrierShipmentAdapter Clone() => new AmazonSWAShipmentAdapter(this);
    }
}
