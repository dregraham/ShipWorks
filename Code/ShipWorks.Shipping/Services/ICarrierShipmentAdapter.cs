namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Adapter for carrier specific fields that are actually common across them
    /// </summary>
    public interface ICarrierShipmentAdapter
    {
        /// <summary>
        /// Id of the carrier account
        /// </summary>
        long? AccountId { get; set; }
    }
}