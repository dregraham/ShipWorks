using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Environment
{
    /// <summary>
    /// FedEx specific carrier settings repository
    /// </summary>
    public interface IFedExSettingsRepository : ICarrierSettingsRepository, IReadOnlyCarrierAccountRetriever<IFedExAccountEntity>
    {

    }
}