using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    [Component]
    public class UpsUtilityWrapper : IUpsUtility
    {
        public string FetchAndSaveUpsAccessKey(UpsAccountEntity upsAccount, string upsLicense)
        {
            return UpsUtility.FetchAndSaveUpsAccessKey(upsAccount, upsLicense);
        }

        public string GetAccessLicenseText()
        {
            return UpsUtility.GetAccessLicenseText();
        }
    }
}