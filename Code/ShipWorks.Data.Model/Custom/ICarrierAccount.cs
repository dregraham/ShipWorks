namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Allow carrier accounts to be used interchangably
    /// </summary>
    public interface ICarrierAccount
    {
        /// <summary>
        /// Get the id of the account
        /// </summary>
        long AccountId { get; }

        /// <summary>
        /// Get a description of the account
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        int ShipmentType { get; }
    }
}
