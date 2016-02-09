using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    public class OnTracShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager) : base(shipment, shipmentTypeManager, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.OnTrac, nameof(shipment.OnTrac));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.OnTrac.OnTracAccountID; }
            set { Shipment.OnTrac.OnTracAccountID = value.GetValueOrDefault(); }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public override bool SupportsAccounts
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public override bool SupportsPackageTypes => true;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType
        {
            get { return Shipment.OnTrac.Service; }
            set { Shipment.OnTrac.Service = value; }
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
            if (Shipment.InsuranceProvider != shippingSettings.OnTracInsuranceProvider)
            {
                Shipment.InsuranceProvider = shippingSettings.OnTracInsuranceProvider;
            }

            if (Shipment.OnTrac.InsurancePennyOne != shippingSettings.OnTracInsurancePennyOne)
            {
                Shipment.OnTrac.InsurancePennyOne = shippingSettings.OnTracInsurancePennyOne;
            }
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            OnTracServiceType? service = tag as OnTracServiceType?;

            return service.HasValue ? (int?) service.Value : base.GetServiceTypeAsIntFromTag(tag);
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            int? service = GetServiceTypeAsIntFromTag(rate.Tag);
            if (service.HasValue)
            {
                Shipment.OnTrac.Service = service.Value;
            }
        }
    }
}
