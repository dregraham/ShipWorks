using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Overstock;

namespace ShipWorks.Stores.UI.Platforms.Overstock
{
    /// <summary>
    /// ViewModel for setting up a Overstock store
    /// </summary>
    [Component]
    public class OverstockStoreSetupControlViewModel : IOverstockStoreSetupControlViewModel, INotifyPropertyChanged
    {
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IOverstockWebClient webClient;
        private string username;
        private string password;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockStoreSetupControlViewModel(IOverstockWebClient webClient, IEncryptionProviderFactory encryptionProviderFactory, IMessageHelper messageHelper)
        {
            this.webClient = webClient;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Gets or sets the User name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Username
        {
            get { return username; }
            set { handler.Set(nameof(Username), ref username, value); }
        }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Password
        {
            get { return password; }
            set { handler.Set(nameof(Password), ref password, value); }
        }

        /// <summary>
        /// Load the store into the view model
        /// </summary>
        public void Load(OverstockStoreEntity store)
        {
            Username = store.Username;
            Password = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.Username).Decrypt(store.Password);
        }

        /// <summary>
        /// Save the API user and Secret to the store
        /// </summary>
        public bool Save(OverstockStoreEntity store)
        {
            // Use a throw away store to test entered credentials.
            OverstockStoreEntity testStore = new OverstockStoreEntity()
            {
                Username = Username,
                Password = Password
            };

            bool result = webClient.TestConnection(testStore).Result;

            if (!result)
            {
                messageHelper.ShowError("Unable to authenticate credentials.");
                return false;
            }

            store.Username = Username;
            store.Password = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.Username).Encrypt(Password);

            return true;
        }
    }
}