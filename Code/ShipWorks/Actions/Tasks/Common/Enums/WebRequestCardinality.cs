using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Actions.Tasks.Common.Enums
{
    /// <summary>
    /// Defines how many requests are made
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum WebRequestCardinality
    {
        [Description("single request")]
        SingleRequest = 0,

        [Description("request for each {0} in the filter")]
        OneRequestPerFilterResult = 1,

        [Description("request using template results as the {0} body")]
        OneRequestPerTemplateResult = 2
    }
}