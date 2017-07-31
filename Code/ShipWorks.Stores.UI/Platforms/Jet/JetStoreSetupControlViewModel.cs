using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;

namespace ShipWorks.Stores.UI.Platforms.Jet
{
    /// <summary>
    /// ViewModel for setting up a Jet store
    /// </summary>
    public class JetStoreSetupControlViewModel : IJetStoreSetupControlViewModel, INotifyPropertyChanged
    {
        private readonly IJetWebClient webClient;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IMessageHelper messageHelper;
        private string apiUser;
        private string secret;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetStoreSetupControlViewModel(IJetWebClient webClient, IEncryptionProviderFactory encryptionProviderFactory, IMessageHelper messageHelper)
        {
            this.webClient = webClient;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Gets or sets the Api User
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ApiUser
        {
            get { return apiUser; }
            set { handler.Set(nameof(ApiUser), ref apiUser, value); }
        }

        /// <summary>
        /// Gets or sets the Secret
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Secret
        {
            get { return secret; }
            set { handler.Set(nameof(Secret), ref secret, value); }
        }

        /// <summary>
        /// Load the store into the view model
        /// </summary>
        public void Load(JetStoreEntity store)
        {
            ApiUser = store.ApiUser;
            Secret = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ApiUser).Decrypt(store.Secret);
        }

        /// <summary>
        /// Save the Api user and Secret to the store
        /// </summary>
        public bool Save(JetStoreEntity store)
        {
            GenericResult<string> result = webClient.GetToken(ApiUser, Secret);

            if (result.Failure)
            {
                messageHelper.ShowError(result.Message);
                return false;
            }
            
            store.ApiUser = ApiUser;
            store.Secret = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ApiUser).Encrypt(Secret);

            return true;
        }
    }
}