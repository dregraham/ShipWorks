namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Shipping Status for ChannelAdvisor's REST API
    /// </summary>
    public enum ChannelAdvisorRestShippingStatus
    {
        Unshipped = 0,

        Shipped = 1,

        PartiallyShipped = 2,

        PendingShipment = 4,

        Canceled = 8,

        ThirdPartyManaged = 16,

        Unknown = 99
    }
}