using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// ChannelAdvisor's adjustment reason enum
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorAdjustmentReason
    {
        GeneralAdjustment = 100,

        ItemNotAvailable = 101,

        CustomerReturnedItem = 102,

        CouldNotShip = 103,

        AlternateItemProvided = 104,

        BuyerCancelled = 105,

        CustomerExchange = 106,

        MerchandiseNotReceived = 107,

        ShippingAddressUndeliverable = 108
    }
}