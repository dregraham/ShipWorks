using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Enum for order archiving by order date comparison
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum OrderArchiverOrderDataComparisonType
    {
        [Description("Order dates less than given date.")]
        LessThan = 0,

        [Description("Order dates greater than or equal to given date.")]
        GreaterThanOrEqual = 1
    }
}
