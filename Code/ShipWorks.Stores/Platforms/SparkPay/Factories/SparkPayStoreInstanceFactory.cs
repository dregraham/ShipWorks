using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.PlatforInterfaces;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayStoreInstanceFactory : IStoreInstanceFactory
    {
        public StoreEntity CreateStoreInstance()
        {
            return new SparkPayStoreEntity
            {
                Enabled = true,
                SetupComplete = false,
                Edition = "",

                TypeCode = (int)StoreTypeCode.SparkPay,
                CountryCode = "US",

                AutoDownload = false,
                AutoDownloadMinutes = 2,
                AutoDownloadOnlyAway = true,

                ComputerDownloadPolicy = "",

                ManualOrderPrefix = "",
                ManualOrderPostfix = "-M",

                DefaultEmailAccountID = -1,

                AddressValidationSetting = (int)AddressValidationStoreSettingType.ValidateAndApply,

                Token = "",
                StoreUrl = "",
                StoreName = "My SparkPay Store",
            };
        }
    }
}
