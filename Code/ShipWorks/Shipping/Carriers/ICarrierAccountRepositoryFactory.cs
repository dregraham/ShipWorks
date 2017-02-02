using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Factory for getting carrier account repositories
    /// </summary>
    /// <remarks>
    /// Autofac gets us close to this ability but it's much easier to use a factory in tests than to mock
    /// out instances of IIndex[key]</remarks>
    public interface ICarrierAccountRepositoryFactory
    {
        /// <summary>
        /// Get a carrier account repository
        /// </summary>
        ICarrierAccountRepository<ICarrierAccount, ICarrierAccount> Get(ShipmentTypeCode shipmentType);
    }
}
