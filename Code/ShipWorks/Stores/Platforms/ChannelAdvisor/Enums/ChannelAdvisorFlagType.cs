using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Interapptive.Shared.Utility;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// An enumeration for the types of flags that can be associated with ChannelAdvisor
    /// orders.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorFlagType
    {
        /// <summary>
        /// According to ChannelAdvisor, flag styles are string values in the API, so they 
        /// can easily add more types without needing to update their WSDL. When using the
        /// Enum.TryParse method, if an new/unknown flag type is encountered, the NoFlag
        /// value will be assigned to the output parameter since it is the first entry 
        /// in the enumeration.
        /// </summary>
        [Description("None")]
        NoFlag = 0,

        [Description("Exclamation Point")]
        [ImageResource("cancel16")]
        ExclamationPoint = 1,

        [Description("Question Mark")]
        [ImageResource("help2_16")]
        QuestionMark = 2,

        [Description("Not Availalble")]
        [ImageResource("not_available")]
        NotAvailable = 3,

        [Description("Price")]
        [ImageResource("currency_dollar_green16")]
        Price = 4,

        [Description("Blue")]
        [ImageResource("flag_blue")]
        BlueFlag = 5,

        [Description("Green")]
        [ImageResource("flag_green")]
        GreenFlag = 6,

        [Description("Red")]
        [ImageResource("flag_red")]
        RedFlag = 7,

        [Description("Yellow")]
        [ImageResource("flag_yellow")]
        YellowFlag = 8,

        [Description("Item Copied")]
        [ImageResource("copy")]
        ItemCopied = 9
    }
}
