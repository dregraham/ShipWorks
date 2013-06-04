using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Ebay.Enums
{
    /// <summary>
    /// QuestionType for sending messages to ebay members
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EbaySendMessageType
    {
        [Description("Custom")]
        CustomCode = 0,

        [Description("As Configured in My eBay")]
        CustomizedSubject = 1,

        [Description("General")]
        General = 2,

        [Description("Shipping Multiple Items")]
        MultipleItemShipping = 3,

        [Description("None")]
        None = 4,

        [Description("Payment")]
        Payment = 5,

        [Description("Shipping")]
        Shipping = 6
    }
}
