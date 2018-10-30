using System.Reflection;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// View model for the from section
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public interface IFromViewModel : IOrderLookupViewModel
    {
        /// <summary>
        /// Field layout provider
        /// </summary>
        IOrderLookupFieldLayoutProvider FieldLayoutProvider { get; }
    }
}
