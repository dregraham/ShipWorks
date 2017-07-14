namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Payment status for ChannelAdvisor's REST API
    /// </summary>
    public enum ChannelAdvisorRestPaymentStatus
    {
        NotYetSubmitted = 0,

        Cleared = 1,

        Submitted = 2,

        Failed = 4,

        Deposited = 8,

        Unknown = 99
    }
}