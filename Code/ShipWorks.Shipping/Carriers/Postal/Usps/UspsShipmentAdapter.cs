using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Adapter for Usps specific shipment information
    /// </summary>
    public class UspsShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private UspsShipmentAdapter(UspsShipmentAdapter adapterToCopy) : base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager) : base(shipment, shipmentTypeManager, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal, nameof(shipment.Postal));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal.Usps, nameof(shipment.Postal.Usps));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the Usps account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.Postal.Usps.UspsAccountID; }
            set { Shipment.Postal.Usps.UspsAccountID = value.GetValueOrDefault(); }
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
            get { return Shipment.Postal.Service; }
            set { Shipment.Postal.Service = value; }
        }

        /// <summary>
        /// Update the insurance fields on the shipment and packages
        /// </summary>
        public override void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            Shipment.InsuranceProvider = shippingSettings.UspsInsuranceProvider;
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

            PostalRateSelection selection = rate.Tag as PostalRateSelection;

            return selection != null &&
                (int) selection.ServiceType == ServiceType &&
                (int) selection.ConfirmationType == Shipment.Postal.Confirmation;
        }

        /// <summary>
        /// Perform the service update
        /// </summary>
        protected override void UpdateServiceFromRate(RateResult rate)
        {
            PostalRateSelection rateSelection = rate.Tag as PostalRateSelection;

            if (rateSelection != null)
            {
                Shipment.Postal.Service = (int) rateSelection.ServiceType;
                Shipment.Postal.Confirmation = (int) rateSelection.ConfirmationType;
            }
        }

        /// <summary>
        /// Perform the clone of the adapter using the cloned shipment
        /// </summary>
        /// <returns></returns>
        public override ICarrierShipmentAdapter Clone() => new UspsShipmentAdapter(this);
    }
}
