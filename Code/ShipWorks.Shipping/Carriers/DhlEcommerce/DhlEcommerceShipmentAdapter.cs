using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce Shipment Adapter
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentAdapter), ShipmentTypeCode.DhlEcommerce)]
    public class DhlEcommerceShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private DhlEcommerceShipmentAdapter(DhlEcommerceShipmentAdapter adapterToCopy) :
            base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            ICustomsManager customsManager, IStoreManager storeManager) :
            base(shipment, shipmentTypeManager, customsManager, storeManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlEcommerce, nameof(shipment.DhlEcommerce));
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.DhlEcommerce.DhlEcommerceAccountID; }
            set { Shipment.DhlEcommerce.DhlEcommerceAccountID = value.GetValueOrDefault(); }
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
            get { return Shipment.DhlEcommerce.Service; }
            set { Shipment.DhlEcommerce.Service = value; }
        }

        /// <summary>
        /// Service type name
        /// </summary>
        public override string ServiceTypeName => EnumHelper.GetDescription((DhlEcommerceServiceType) ServiceType);
        
        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            Shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

            if (Shipment.InsuranceProvider != shippingSettings.DhlEcommerceInsuranceProvider)
            {
                Shipment.InsuranceProvider = shippingSettings.DhlEcommerceInsuranceProvider;
            }

            if (Shipment.DhlEcommerce.InsurancePennyOne != shippingSettings.DhlEcommerceInsurancePennyOne)
            {
                Shipment.DhlEcommerce.InsurancePennyOne = shippingSettings.DhlEcommerceInsurancePennyOne;
            }
        }

        /// <summary>
        /// Get the service type as an integer from the given tag
        /// </summary>
        protected override int? GetServiceTypeAsIntFromTag(object tag)
        {
            string serviceName = tag as string;
            if (serviceName == string.Empty)
            {
                return base.GetServiceTypeAsIntFromTag(tag);
            }

            return (int) EnumHelper.GetEnumByApiValue<DhlEcommerceServiceType>(serviceName);
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            int? service = GetServiceTypeAsIntFromTag(rate.Tag);
            if (service.HasValue)
            {
                Shipment.DhlEcommerce.Service = service.Value;
            }
        }

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        public override ICarrierShipmentAdapter Clone() => new DhlEcommerceShipmentAdapter(this);
    }
}
