using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Flag type for ChannelAdvisor's REST API
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorRestFlagType
    {
        [Description("Not Specified")]
        NotSpecified = -9999,

        [Description("Item Copied")]
        [ImageResource("copy")]
        ItemCopied = -2,

        [Description("Exclamation Point")]
        [ImageResource("cancel16")]
        ExclamationPoint = -1,

        [Description("None")]
        NoFlag = 0,

        [Description("Red")]
        [ImageResource("flag_red")]
        RedFlag = 1,

        [Description("Question Mark")]
        [ImageResource("help2_16")]
        QuestionMark = 2,

        [Description("Not Available")]
        [ImageResource("not_available")]
        NotAvailable = 3,

        [Description("Price")]
        [ImageResource("currency_dollar_green16")]
        Price = 4,

        [Description("Yellow")]
        [ImageResource("flag_yellow")]
        YellowFlag = 5,

        [Description("Green")]
        [ImageResource("flag_green")]
        GreenFlag = 6,

        [Description("Blue")]
        [ImageResource("flag_blue")]
        BlueFlag = 7,
    }
}