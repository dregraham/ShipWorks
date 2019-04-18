using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Repository for AmazonSWA accounts
    /// </summary>
    public interface IAmazonSWAAccountRepository : ICarrierAccountRepository<AmazonSWAAccountEntity, IAmazonSWAAccountEntity>
    {
    }
}