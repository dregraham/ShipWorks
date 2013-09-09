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
        [Description("One Time")]
        SingleRequest = 0,

        [Description("Once for each {0} in the filter")]
        OneRequestPerFilterResult = 1,

        [Description("Using the results of processing a template")]
        OneRequestPerTemplateResult = 2
    }
}