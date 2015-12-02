using System;
using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages
{
    public class YahooApiAccountPageViewModel : YahooApiAccountViewModel, INotifyPropertyChanged
    {
        private readonly IStoreTypeManager storeTypeManager;
        private string helpUrl;

        public YahooApiAccountPageViewModel(IStoreTypeManager storeTypeManager, Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient) : base(storeWebClient)
        {
            this.storeTypeManager = storeTypeManager;
            YahooStoreType storeType = storeTypeManager.GetType(StoreTypeCode.Yahoo) as YahooStoreType;
            HelpUrl = storeType?.AccountSettingsHelpUrl;
        }

        [Obfuscation(Exclude = true)]
        public string HelpUrl
        {
            get { return helpUrl; }
            set { handler.Set(nameof(HelpUrl), ref helpUrl, value); }
        }

        public void Load(YahooStoreEntity storeEntity)
        {
            HelpUrl = ((YahooStoreType) storeTypeManager.GetType(StoreTypeCode.Yahoo)).AccountSettingsHelpUrl;
            ValidateBackupOrderNumber();
        }
    }
}