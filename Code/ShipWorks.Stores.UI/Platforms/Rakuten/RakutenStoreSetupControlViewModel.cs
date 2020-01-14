using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Rakuten;

namespace ShipWorks.Stores.UI.Platforms.Rakuten
{
    /// <summary>
    /// ViewModel for setting up a Rakuten store
    /// </summary>
    [Component]
    public class RakutenStoreSetupControlViewModel : IRakutenStoreSetupControlViewModel, INotifyPropertyChanged
    {
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IRakutenWebClient webClient;
        private string apiKey;
        private string marketplaceID;
        private string shopUrl;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenStoreSetupControlViewModel(IRakutenWebClient webClient, IEncryptionProviderFactory encryptionProviderFactory, IMessageHelper messageHelper)
        {
            this.webClient = webClient;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Gets or sets the API Key
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ApiKey
        {
            get { return apiKey; }
            set { handler.Set(nameof(ApiKey), ref apiKey, value); }
        }

        /// <summary>
        /// Gets or sets the Marketplace ID
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string MarketplaceID
        {
            get { return marketplaceID; }
            set { handler.Set(nameof(MarketplaceID), ref marketplaceID, value); }
        }

        /// <summary>
        /// Gets or sets the Shop URL
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ShopUrl
        {
            get { return shopUrl; }
            set { handler.Set(nameof(ShopUrl), ref shopUrl, value); }
        }
        /// <summary>
        /// Load the store into the view model
        /// </summary>
        public void Load(RakutenStoreEntity store)
        {
            ApiKey = string.IsNullOrEmpty(store.AuthKey) ? string.Empty :
                encryptionProviderFactory.CreateRakutenEncryptionProvider().Decrypt(store.AuthKey);
            MarketplaceID = string.IsNullOrEmpty(store.MarketplaceID) ? "us" : store.MarketplaceID;
            ShopUrl = store.ShopURL;
        }

        /// <summary>
        /// Save the API user and Secret to the store
        /// </summary>
        public async Task<bool> Save(RakutenStoreEntity store)
        {
            if (ApiKey.IsNullOrWhiteSpace())
            {
                messageHelper.ShowError("Please enter a Rakuten Auth Key");
                return false;
            }

            var authKey = encryptionProviderFactory.CreateRakutenEncryptionProvider().Encrypt(ApiKey);

            // Use a throw away store to test entered credentials.
            RakutenStoreEntity testStore = new RakutenStoreEntity()
            {
                AuthKey = authKey,
                MarketplaceID = MarketplaceID,
                ShopURL = ShopUrl
            };

            var result = await webClient.TestConnection(testStore).ConfigureAwait(false);

            if (result.Failure)
            {
                messageHelper.ShowError($"Unable to authenticate credentials: {result.Exception.Message}");
                return false;
            }

            store.AuthKey = authKey;
            store.MarketplaceID = MarketplaceID;
            store.ShopURL = ShopUrl;

            return true;
        }
    }
}