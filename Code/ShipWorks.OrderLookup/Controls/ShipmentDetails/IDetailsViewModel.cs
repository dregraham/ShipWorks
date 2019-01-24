using System.Reflection;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for order lookup shipment details
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public interface IDetailsViewModel : IOrderLookupViewModel
    {
    }
}