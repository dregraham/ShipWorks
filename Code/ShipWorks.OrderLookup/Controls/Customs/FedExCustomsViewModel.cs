using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    /// <summary>
    /// View model for the FedExCustomsControl
    /// </summary>
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.FedEx)]
    [WpfView(typeof(FedExCustomsControl))]
    public class FedExCustomsViewModel : GenericCustomsViewModel
    {
        private Dictionary<int, string> customsExportFilingOptions;

        /// <summary>
        /// Ctor
        /// </summary>
        public FedExCustomsViewModel(IOrderLookupShipmentModel shipmentModel, 
            IShipmentTypeManager shipmentTypeManager,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, fieldLayoutProvider)
        {
            CustomsExportFilingOptions = EnumHelper.GetEnumList<FedExCustomsExportFilingOption>().ToDictionary(x => (int) x.Value, x => x.Description);
        }

        /// <summary>
        /// List of available customs export filing options
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> CustomsExportFilingOptions
        {
            get => customsExportFilingOptions;
            set => Handler.Set(nameof(CustomsExportFilingOptions), ref customsExportFilingOptions, value);
        }
    }
}