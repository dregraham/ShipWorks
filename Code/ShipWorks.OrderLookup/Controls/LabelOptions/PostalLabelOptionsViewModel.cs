using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.LabelOptions
{
    /// <summary>
    /// View model for the PostalLabelOptionsControl
    /// </summary>
    [KeyedComponent(typeof(ILabelOptionsViewModel), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(ILabelOptionsViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(PostalLabelOptionsControl))]
    [Obfuscation(Exclude = true, StripAfterObfuscation = true, ApplyToMembers = true)]
    public class PostalLabelOptionsViewModel : GenericLabelOptionsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalLabelOptionsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, fieldLayoutProvider)
        { }
    }
}
