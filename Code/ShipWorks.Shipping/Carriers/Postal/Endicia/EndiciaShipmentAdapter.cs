using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Adapter for Endicia specific shipment information
    /// </summary>
    public class EndiciaShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        private EndiciaShipmentAdapter(EndiciaShipmentAdapter adapterToCopy) : base(adapterToCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager,
            ICustomsManager customsManager, IStoreManager storeManager) :
            base(shipment, shipmentTypeManager, customsManager, storeManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal, nameof(shipment.Postal));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal.Endicia, nameof(shipment.Postal.Endicia));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the Endicia account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.Postal.Endicia.EndiciaAccountID; }
            set { Shipment.Postal.Endicia.EndiciaAccountID = value.GetValueOrDefault(); }
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
            Shipment.InsuranceProvider = shippingSettings.EndiciaInsuranceProvider;
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
        /// For rates that are not selectable, find their first child that is.
        /// </summary>
        public override RateResult GetChildRateForRate(RateResult parentRate, IEnumerable<RateResult> rates)
        {
            RateResult childRate = parentRate;
            PostalRateSelection selection = parentRate.Tag as PostalRateSelection;

            if (!parentRate.Selectable && selection != null)
            {
                childRate = rates.FirstOrDefault(
                    r => r.Selectable && ((PostalRateSelection) r.Tag).ServiceType == selection.ServiceType);
            }

            return childRate;
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
        public override ICarrierShipmentAdapter Clone() => new EndiciaShipmentAdapter(this);
    }
}
