using ShipWorks.Core.UI.SandRibbon;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    public interface IShippingRibbonActions
    {
        IRibbonButton CreateLabel { get; }

        IRibbonButton Void { get; }

        IRibbonButton Return { get; }

        IRibbonButton Reprint { get; }

        IRibbonButton ShipAgain { get; }
    }
}