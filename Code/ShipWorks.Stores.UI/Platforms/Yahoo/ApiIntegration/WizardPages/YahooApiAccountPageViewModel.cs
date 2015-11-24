using System;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages
{
    public class YahooApiAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;
        private string helpUrl;
        private long backupOrderNumber;
        private YahooStoreEntity store;
        private YahooOrderNumberValidation isValid;

        public YahooApiAccountPageViewModel(IStoreTypeManager storeTypeManager, Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient)
        {
            this.storeTypeManager = storeTypeManager;
            this.storeWebClient = storeWebClient;
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

        [Obfuscation(Exclude = true)]
        public YahooOrderNumberValidation IsValid
        {
            get { return isValid; }
            set { handler.Set(nameof(IsValid), ref isValid, value); }
        }

        public void Load(YahooStoreEntity storeEntity)
        {
            store = storeEntity;
            HelpUrl = ((YahooStoreType) storeTypeManager.GetType(StoreTypeCode.Yahoo)).AccountSettingsHelpUrl;
        }

        public string Save(YahooStoreEntity store)
        {
            store.YahooStoreID = YahooStoreID;
            store.AccessToken = AccessToken;
            store.BackupOrderNumber = BackupOrderNumber;

            if (string.IsNullOrWhiteSpace(YahooStoreID))
            {
                return "Please enter your Store URL";
            }

            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                return "Please enter your Access Token";
            }

            IYahooApiWebClient client = storeWebClient(store);

            string response = client.GetOrderRange(BackupOrderNumber);

            if (response.Contains("<Code>20021</Code>") || response.Contains("<Code>10402</Code>"))
            {
                //show invalid order number warning
            }
            
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