using System.ComponentModel;
using System.Reflection;
using ShipWorks.Core.UI;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages
{
    public class YahooAccountSettingsViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;

        public YahooAccountSettingsViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        [Obfuscation(Exclude = true)]
        public string YahooStoreID
        {
            get { return yahooStoreID; }
            set { handler.Set(nameof(YahooStoreID), ref yahooStoreID, value); }
        }

        [Obfuscation(Exclude = true)]
        public string AccessToken
        {
            get { return accessToken; }
            set { handler.Set(nameof(AccessToken), ref accessToken, value); }
        }
    }
}