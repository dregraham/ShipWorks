using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.Custom;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Account description factory
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountDescription), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressAccountDescription : ICarrierAccountDescription
    {
        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public string GetDefaultAccountDescription(ICarrierAccount account)
        {
            ShipEngineAccountEntity dhlAccount = account as ShipEngineAccountEntity;
            MethodConditions.EnsureArgumentIsNotNull(dhlAccount, "DHL Account");

            string[] descriptionComponents = { dhlAccount.AccountNumber.ToString(), dhlAccount.Street1, dhlAccount.PostalCode };

            return string.Join(",", descriptionComponents.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}
