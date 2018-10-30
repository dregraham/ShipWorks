using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Not supported shipment details viewmodel
    /// </summary>
    [Component]
    [WpfView(typeof(NotSupportedShipmentControl))]
    internal class NotSupportedShipmentDetailsViewModel : OrderLookupViewModelBase, IDetailsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NotSupportedShipmentDetailsViewModel(IOrderLookupShipmentModel shipmentModel,
            ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider) : base(shipmentModel)
        {
            Providers = carrierShipmentAdapterOptionsProvider.GetProviders(shipmentModel.ShipmentAdapter, shipmentModel.OriginalShipmentTypeCode);
        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.ShipmentDetails;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string Title { get; protected set; } = "Shipment Details";

        /// <summary>
        /// Error Message of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage => $"Shipping with {EnumHelper.GetDescription(ShipmentModel.ShipmentAdapter.ShipmentTypeCode)} is not supported in Order Lookup mode.";

        /// <summary>
        /// Shipment type code
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode ShipmentTypeCode
        {
            get => ShipmentModel.ShipmentAdapter?.ShipmentTypeCode ?? ShipmentTypeCode.None;
            set
            {
                if (value != ShipmentModel.ShipmentAdapter.ShipmentTypeCode)
                {
                    ShipmentModel.ChangeShipmentType(value);
                }
            }
        }

        /// <summary>
        /// Collection of Providers
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<ShipmentTypeCode, string> Providers
        {
            get;
            private set;
        }
    }
}
