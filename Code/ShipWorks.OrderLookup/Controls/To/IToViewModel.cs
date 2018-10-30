using System.Reflection;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// View model for the To section
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public interface IToViewModel : IOrderLookupViewModel
    {
        /// <summary>
        /// Field layout provider
        /// </summary>
        IOrderLookupFieldLayoutProvider FieldLayoutProvider { get; }
    }
}
