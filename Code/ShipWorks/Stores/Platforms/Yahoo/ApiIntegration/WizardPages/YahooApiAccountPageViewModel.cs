using System.ComponentModel;
using System.Net;
using System.Reflection;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages
{
    public class YahooApiAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;
        private string helpUrl;
        private long backupOrderNumber;

        public YahooApiAccountPageViewModel(IStoreTypeManager storeTypeManager)
        {
            this.storeTypeManager = storeTypeManager;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            YahooStoreType storeType = storeTypeManager.GetType(StoreTypeCode.Yahoo) as YahooStoreType;
            HelpUrl = storeType?.AccountSettingsHelpUrl;
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

        [Obfuscation(Exclude = true)]
        public string HelpUrl
        {
            get { return helpUrl; }
            set { handler.Set(nameof(HelpUrl), ref helpUrl, value); }
        }

        [Obfuscation(Exclude = true)]
        public long BackupOrderNumber
        {
            get { return backupOrderNumber; }
            set { handler.Set(nameof(BackupOrderNumber), ref backupOrderNumber, value); }
        }

        public void Load(YahooStoreType store)
        {
            HelpUrl = store.AccountSettingsHelpUrl;
        }

        public string Save(YahooStoreEntity store)
        {
            store.YahooStoreID = YahooStoreID;
            store.AccessToken = AccessToken;
            store.BackupOrderNumber = BackupOrderNumber;

            if (string.IsNullOrEmpty(YahooStoreID))
            {
                return "Please enter your Store URL";
            }

            if (string.IsNullOrEmpty(AccessToken))
            {
                return "Please enter your Access Token";
            }

            //todo: Check given order number
            
            YahooApiWebClient client = new YahooApiWebClient(store);

            try
            {
                client.ValidateCredentials();
            }
            catch (WebException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}