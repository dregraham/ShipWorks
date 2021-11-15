using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    /// <summary>
    /// View model for the UpsCustomsControl
    /// </summary>
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(UpsCustomsControl))]
    public class UpsCustomsViewModel : GenericCustomsViewModel
    {
        private Dictionary<int, string> customsRecipientTINType;

        /// <summary>
        /// Ctor
        /// </summary>
        public UpsCustomsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, fieldLayoutProvider)
        {
            CustomsRecipientTINType = EnumHelper.GetEnumList<UpsCustomsRecipientTINType>().ToDictionary(x => (int) x.Value, x => x.Description);
        }

        /// <summary>
        /// List of available customs TIN Types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> CustomsRecipientTINType
        {
            get => customsRecipientTINType;
            set => Handler.Set(nameof(CustomsRecipientTINType), ref customsRecipientTINType, value);
        }
    }
}
