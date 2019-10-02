using System.Collections.ObjectModel;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.UI.Settings
{
    [Component(RegistrationType.Self)]
    public class BestRateAccountSettingsViewModel
    {
        public BestRateAccountSettingsViewModel()
        {
            Carriers = new ObservableCollection<BestRateCarrier>();
            Carriers.Add(new BestRateCarrier()
            {
                Name = "USPS",
                ShipmentType = ShipmentTypeCode.Usps,
                Accounts = new ObservableCollection<BestRateAccount>()
                {
                    new BestRateAccount()
                    {
                        AccountID = "12312",
                        IsActive = true
                    },
                    new BestRateAccount()
                    {
                        AccountID = "7890192",
                        IsActive = true
                    },                    new BestRateAccount()
                    {
                        AccountID = "6789732189",
                        IsActive = true
                    },
                }

            });
            Carriers.Add(new BestRateCarrier()
            {
                Name = "UPS",
                ShipmentType = ShipmentTypeCode.UpsOnLineTools,
                Accounts = new ObservableCollection<BestRateAccount>()
                {
                    new BestRateAccount()
                    {
                        AccountID = "12312",
                        IsActive = true
                    },
                    new BestRateAccount()
                    {
                        AccountID = "1254",
                        IsActive = true
                    },                    new BestRateAccount()
                    {
                        AccountID = "24313125",
                        IsActive = true
                    },                    new BestRateAccount()
                    {
                        AccountID = "213",
                        IsActive = true
                    },                    new BestRateAccount()
                    {
                        AccountID = "789",
                        IsActive = true
                    }                    
                }
            });
        }

        public ObservableCollection<BestRateCarrier> Carriers { get; set; }
    }
}
