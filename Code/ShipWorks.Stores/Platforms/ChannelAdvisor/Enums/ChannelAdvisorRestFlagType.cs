namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Flag type for ChannelAdvisor's REST API
    /// </summary>
    public enum ChannelAdvisorRestFlagType
    {
        NotSpecified = -9999,

        ItemCopied = -2,

        ExclamationPoint = -1,

        NoFlag = 0,

        RedFlag = 1,

        QuestionMark = 2,

        NotAvailable = 3,

        Price = 4,

        YellowFlag = 5,

        GreenFlag = 6,

        BlueFlag = 7,

        Unknown = 99
    }
}