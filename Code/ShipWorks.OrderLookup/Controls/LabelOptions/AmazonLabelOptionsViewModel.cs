using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.LabelOptions
{
    /// <summary>
    /// View model for the OrderLookupLabelOptionsViewModel
    /// </summary>
    [KeyedComponent(typeof(ILabelOptionsViewModel), ShipmentTypeCode.Amazon)]
    [WpfView(typeof(AmazonLabelOptionsControl))]
    public class AmazonLabelOptionsViewModel : GenericLabelOptionsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelOptionsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager) : base(shipmentModel, shipmentTypeManager)
        {
        }

        /// <summary>
        /// Exclude EPL because amazon does not support it
        /// </summary>
        protected override bool ShouldIncludeLabelFormatInList(ShipmentEntity shipment, ThermalLanguage labelFormat)
        {
            if (labelFormat == ThermalLanguage.EPL)
            {
                return false;
            }

            return base.ShouldIncludeLabelFormatInList(shipment, labelFormat);
        }
    }
}
