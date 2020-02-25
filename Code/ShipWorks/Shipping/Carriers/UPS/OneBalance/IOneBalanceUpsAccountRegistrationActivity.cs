using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

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
        Result Execute(UpsAccountEntity account);
    }
}
