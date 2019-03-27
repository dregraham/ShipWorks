using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// ShipEngine Account description factory
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountDescription), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWAAccountDescription : ICarrierAccountDescription
    {
        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public string GetDefaultAccountDescription(ICarrierAccount carrierAccount)
        {
            AmazonSWAAccountEntity account = carrierAccount as AmazonSWAAccountEntity;
            MethodConditions.EnsureArgumentIsNotNull(account, "account");

            string[] descriptionComponents = { account.Street1, account.PostalCode };

            return string.Join(",", descriptionComponents.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}
