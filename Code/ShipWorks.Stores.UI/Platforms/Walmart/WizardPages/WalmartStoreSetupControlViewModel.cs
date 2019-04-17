using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart;

namespace ShipWorks.Stores.UI.Platforms.Walmart.WizardPages
{
    /// <summary>
    /// Logic for the WalmartStoreSetupControl
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.UI.Platforms.Walmart.WizardPages.IWalmartStoreSetupControlViewModel" />
    [Component]
    public class WalmartStoreSetupControlViewModel : IWalmartStoreSetupControlViewModel, INotifyPropertyChanged
    {
        private readonly IWalmartWebClient webClient;
        private readonly IEncryptionProvider encryptionProvider;
        private string clientID;
        private string clientSecret;
        private readonly PropertyChangedHandler handler;
        private bool updatingClientSecret;
        private ICommand updateClientSecretCommand;
        private bool isNewStore;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartStoreSetupControlViewModel"/> class.
        /// </summary>
        public WalmartStoreSetupControlViewModel(IWalmartWebClient webClient,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.webClient = webClient;
            encryptionProvider = encryptionProviderFactory.CreateWalmartEncryptionProvider();
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            IsNewStore = true;
            UpdateClientSecretCommand = new RelayCommand(OnUpdateClientSecret);
        }

        /// <summary>
        /// Whether or not the store is being setup for the first time
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsNewStore
        {
            get { return isNewStore; }
            set { handler.Set(nameof(IsNewStore), ref isNewStore, value); }
        }

        /// <summary>
        /// Client ID issued by Walmart
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ClientID
        {
            get { return clientID; }
            set { handler.Set(nameof(ClientID), ref clientID, value); }
        }

        /// <summary>
        /// Client secret issued by Walmart
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ClientSecret
        {
            get { return clientSecret; }
            set { handler.Set(nameof(ClientSecret), ref clientSecret, value); }
        }

        /// <summary>
        /// Whether or not the client secret is being updated.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool UpdatingClientSecret
        {
            get { return updatingClientSecret; }
            set { handler.Set(nameof(UpdatingClientSecret), ref updatingClientSecret, value); }
        }

        /// <summary>
        /// Command to run when updating the client secret.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UpdateClientSecretCommand
        {
            get { return updateClientSecretCommand; }
            set { handler.Set(nameof(UpdateClientSecretCommand), ref updateClientSecretCommand, value); }
        }

        /// <summary>
        /// Validates the credentials entered by the user and saves them if they are valid.
        /// </summary>
        public void Save(WalmartStoreEntity store)
        {
            if (EnsureRequiredFieldsHaveValue())
            {
                store.ClientID = ClientID.Trim();

                if (!string.IsNullOrWhiteSpace(ClientSecret))
                {
                    store.ClientSecret = encryptionProvider.Encrypt(ClientSecret.Trim());
                }

                // Throws if credentials are not valid
                webClient.TestConnection(store);

                if (!IsNewStore)
                {
                    UpdatingClientSecret = false;
                }
            }
        }

        /// <summary>
        /// Loads the credentials for the given store.
        /// </summary>
        public void Load(WalmartStoreEntity store)
        {
            ClientID = store.ClientID;

            IsNewStore = string.IsNullOrWhiteSpace(store.ClientID);
            if (IsNewStore)
            {
                UpdatingClientSecret = true;
            }
        }

        /// <summary>
        /// Ensures the required fields have a value.
        /// </summary>
        private bool EnsureRequiredFieldsHaveValue()
        {
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(ClientID))
            {
                invalidFields.Add("Client ID");
            }

            if (string.IsNullOrWhiteSpace(ClientSecret) && IsNewStore)
            {
                invalidFields.Add("Client secret");
            }

            if (invalidFields.Any())
            {
                throw new WalmartException($"Please enter your\n\t-{string.Join("\n\t-", invalidFields)}");
            }

            return !invalidFields.Any();
        }

        /// <summary>
        /// Called when [update client secret].
        /// </summary>
        private void OnUpdateClientSecret()
        {
            UpdatingClientSecret = true;
        }
    }
}