using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// ShipEngine Account description factory
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountDescription), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressAccountDescription : ICarrierAccountDescription
    {
        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public string GetDefaultAccountDescription(ICarrierAccount carrierAccount)
        {
            DhlExpressAccountEntity account = carrierAccount as DhlExpressAccountEntity;

            if (account?.UspsAccountId != null)
            {
                return "DHL Express from ShipWorks";
            }

            MethodConditions.EnsureArgumentIsNotNull(account, "account");

            string[] descriptionComponents =
                {account?.AccountNumber.ToString(), account?.Street1, account?.PostalCode};

            return string.Join(",", descriptionComponents.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}
