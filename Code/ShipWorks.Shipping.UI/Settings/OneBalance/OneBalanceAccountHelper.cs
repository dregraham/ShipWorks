using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Contains helper methods for interacting with one balance accounts
    /// </summary>
    [Component]
    public class OneBalanceAccountHelper : IOneBalanceAccountHelper
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsRepository;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsRepository;
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> dhlRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAccountHelper(
             ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsRepository,
             ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsRepository,
             ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> dhlRepository)
        {
            this.uspsRepository = uspsRepository;
            this.upsRepository = upsRepository;
            this.dhlRepository = dhlRepository;
        }

        /// <summary>
        /// Get the account from stamps
        /// </summary>
        public GenericResult<IUspsAccountEntity> GetUspsAccount(ShipmentTypeCode shipmentTypeCode)
        {
            var accounts = uspsRepository.AccountsReadOnly;

            if (!accounts.Any())
            {
                return new UspsException($"Getting access to great rates with {EnumHelper.GetDescription(shipmentTypeCode)} from ShipWorks requires a Stamps.com account." +
                    "\n\r\n\rPlease create a Stamps.com account in the USPS portion of Shipping Settings and try again.");
            }

            // If there's only one account it's a One Balance account
            if (accounts.IsCountEqualTo(1))
            {
                return GenericResult.FromSuccess(accounts.First());
            }

            // If there are multiple accounts the one with a ShipEngineCarrierId is the One Balance account
            var account = accounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);

            if (account == null)
            {
                return new UspsException("Unable to determine which USPS account to use. Please call ShipWorks support at 1-314-821-5888");
            }

            return GenericResult.FromSuccess(account);
        }

        /// <summary>
        /// Get the dhl account from the local database
        /// </summary>
        public bool LocalDhlAccountExists() =>
            dhlRepository.AccountsReadOnly.Any(e => e.UspsAccountId != null);

        /// <summary>
        /// Get the ups account from the local database
        /// </summary>
        public bool LocalUpsAccountExists() =>
            upsRepository.AccountsReadOnly.Any(e => e.ShipEngineCarrierId != null);
    }
}
