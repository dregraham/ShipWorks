using System;
using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;

namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages
{
    /// <summary>
    /// View model for the YahooAccountPage
    /// </summary>
    public class YahooApiAccountPageViewModel : YahooApiAccountViewModel, INotifyPropertyChanged
    {
        private string helpUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiAccountPageViewModel"/> class.
        /// </summary>
        /// <param name="storeTypeManager">The store type manager.</param>
        /// <param name="storeWebClient">The store web client.</param>
        public YahooApiAccountPageViewModel(IStoreTypeManager storeTypeManager, Func<YahooStoreEntity, IYahooApiWebClient> storeWebClient) : base(storeWebClient)
        {
            YahooStoreType storeType = storeTypeManager.GetType(StoreTypeCode.Yahoo) as YahooStoreType;
            HelpUrl = storeType?.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// Help link URL
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string HelpUrl
        {
            get { return helpUrl; }
            set { Handler.Set(nameof(HelpUrl), ref helpUrl, value); }
        }

        /// <summary>
        /// Loads the page
        /// </summary>
        /// <param name="storeEntity">The store entity.</param>
        public void Load(YahooStoreEntity storeEntity)
        {
            HandleChanges();
        }
    }
}