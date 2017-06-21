using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public interface IUpsUtility
    {
        string FetchAndSaveUpsAccessKey(UpsAccountEntity upsAccount, string upsLicense);
        string GetAccessLicenseText();
    }
}