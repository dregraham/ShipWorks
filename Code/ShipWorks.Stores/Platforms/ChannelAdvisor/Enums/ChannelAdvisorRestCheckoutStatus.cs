namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Checkout status for ChannelAdvisor's REST API
    /// </summary>
    public enum ChannelAdvisorRestCheckoutStatus
    {
        NotVisited = 0,

        Completed = 1,

        Visited = 2,

        Disabled = 4,

        CompletedAndVisited = 3,

        CompletedOffline = 8,

        OnHold = 16,

        Unknown = 99
    }
}