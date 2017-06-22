using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Interface that represents the static UpsUtility
    /// </summary>
    public interface IUpsUtility
    {
        /// <summary>
        /// Get the global instanced UPS access key
        /// </summary>
        string FetchAndSaveUpsAccessKey(UpsAccountEntity upsAccount, string upsLicense);

        /// <summary>
        /// Get the Ups AccessLicense text
        /// </summary>
        string GetAccessLicenseText();
    }
}