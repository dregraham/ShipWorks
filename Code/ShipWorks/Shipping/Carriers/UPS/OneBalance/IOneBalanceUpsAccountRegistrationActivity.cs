using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    /// <summary>
    /// Activity for registering a UPS account with One Balance
    /// </summary>
    public interface IOneBalanceUpsAccountRegistrationActivity
    {
        /// <summary>
        /// Register a UPS account with One Balance
        /// </summary>
        Task<Result> Execute(UpsAccountEntity account, string deviceIdentity);

        Task<Result> Execute(IUspsAccountEntity uspsOneBalanceAccount, UpsAccountEntity upsAccountToCreate,
                             string deviceIdentity);
    }
}
