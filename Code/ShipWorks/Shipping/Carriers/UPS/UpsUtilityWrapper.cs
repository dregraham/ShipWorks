using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// wrapper for the static UpsUtility
    /// </summary>
    [Component]
    public class UpsUtilityWrapper : IUpsUtility
    {
        /// <summary>
        /// Get the global instanced UPS access key
        /// </summary>
        public string FetchAndSaveUpsAccessKey(UpsAccountEntity upsAccount, string upsLicense)
        {
            return UpsUtility.FetchAndSaveUpsAccessKey(upsAccount, upsLicense);
        }

        /// <summary>
        /// Get the Ups AccessLicense text
        /// </summary>
        public string GetAccessLicenseText()
        {
            return UpsUtility.GetAccessLicenseText();
        }
    }
}