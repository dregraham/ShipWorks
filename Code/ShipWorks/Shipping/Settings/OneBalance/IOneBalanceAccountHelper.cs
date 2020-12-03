using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Settings.OneBalance
{
    /// <summary>
    /// Interface for the OneBalanceAccountHelper
    /// </summary>
    public interface IOneBalanceAccountHelper
    {
        /// <summary>
        /// Get the account from stamps
        /// </summary>
        GenericResult<IUspsAccountEntity> GetUspsAccount(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Get the dhl account from the local database
        /// </summary>
        bool LocalDhlAccountExists();

        /// <summary>
        /// Get the ups account from the local database
        /// </summary>
        bool LocalUpsAccountExists();

        /// <summary>
        /// Get the USPS account that has been used to setup one balance
        /// </summary>
        UspsAccountEntity GetUspsOneBalanceAccount();
    }
}