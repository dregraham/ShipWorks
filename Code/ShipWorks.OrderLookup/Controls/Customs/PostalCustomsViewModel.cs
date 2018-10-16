using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(PostalCustomsControl))]
    public class PostalCustomsViewModel : GenericCustomsViewModel
    {
        private Dictionary<int, string> customsContentTypes;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalCustomsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager) : base(shipmentModel, shipmentTypeManager)
        {
            CustomsContentTypes = EnumHelper.GetEnumList<PostalCustomsContentType>().ToDictionary(x => (int) x.Value, x => x.Description);
        }

        /// <summary>
        /// List of available customs content types for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> CustomsContentTypes
        {
            get => customsContentTypes;
            set => handler.Set(nameof(CustomsContentTypes), ref customsContentTypes, value);
        }
    }
}
