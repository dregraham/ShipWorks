using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interface for the OneBalanceAccountHelper
    /// </summary>
    public interface IOneBalanceAccountHelper
    {
        /// <summary>
        /// Get the account from stamps
        /// </summary>
        GenericResult<IUspsAccountEntity> GetUspsAccount();

        /// <summary>
        /// Get the dhl account from the local database
        /// </summary>
        bool LocalDhlAccountExists();

        /// <summary>
        /// Get the ups account from the local database
        /// </summary>
        bool LocalUpsAccountExists();
    }
}