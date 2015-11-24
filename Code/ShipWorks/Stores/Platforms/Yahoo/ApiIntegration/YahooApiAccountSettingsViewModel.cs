using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiAccountSettingsViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string yahooStoreID;
        private string accessToken;
        private long backupOrderNumber;

        public YahooApiAccountSettingsViewModel()
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
        
        [Obfuscation(Exclude = true)]
        public long BackupOrderNumber
        {
            get { return backupOrderNumber; }
            set { handler.Set(nameof(BackupOrderNumber), ref backupOrderNumber, value); }
        }

        public void Load(YahooStoreEntity store)
        {
            YahooStoreID = store.YahooStoreID;
            AccessToken = store.AccessToken;
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
            YahooApiDownloader downloader = new YahooApiDownloader(store);

            try
            {
                client.ValidateCredentials();
                downloader.ForceDownload();
            }
            catch (WebException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
