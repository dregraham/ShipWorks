using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Enum for the different types of filter definition sources, i.e. who created it
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FilterDefinitionSourceType
    {
        // Regular filters
        [Description("Filter")]
        Filter = 0,

        // Searches
        [Description("Search")]
        Search = 1,

        // Scan based searches
        [Description("Scan")]
        Scan = 2
    }
}
