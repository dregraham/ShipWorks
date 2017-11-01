using ShipWorks.Data.Model.EntityClasses;
using System.Linq;
using ShipWorks.Data.Model.Custom;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// ShipEngine Account description factory
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountDescription), ShipmentTypeCode.Asendia)]
    [KeyedComponent(typeof(ICarrierAccountDescription), ShipmentTypeCode.DhlExpress)]
    public class ShipEngineAccountDescription : ICarrierAccountDescription
    {
        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public string GetDefaultAccountDescription(ICarrierAccount carrierAccount)
        {
            ShipEngineAccountEntity account = carrierAccount as ShipEngineAccountEntity;
            MethodConditions.EnsureArgumentIsNotNull(account, "account");

            string[] descriptionComponents = { account.AccountNumber.ToString(), account.Street1, account.PostalCode };

            return string.Join(",", descriptionComponents.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}
