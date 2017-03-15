using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
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
        private string consumerID;
        private string privateKey;
        private string channelType;
        private readonly PropertyChangedHandler handler;
        private bool updatingPrivateKey;
        private ICommand updatePrivateKeyCommand;
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
            UpdatePrivateKeyCommand = new RelayCommand(OnUpdatePrivateKey);
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
        /// Consumer ID issued by Walmart
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ConsumerID
        {
            get { return consumerID; }
            set { handler.Set(nameof(ConsumerID), ref consumerID, value); }
        }

        /// <summary>
        /// Private key issued by Walmart
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string PrivateKey
        {
            get { return privateKey; }
            set { handler.Set(nameof(PrivateKey), ref privateKey, value); }
        }

        /// <summary>
        /// Channel Type issued by Walmart
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ChannelType
        {
            get { return channelType; }
            set { handler.Set(nameof(ChannelType), ref channelType, value); }
        }

        /// <summary>
        /// Whether or not the private key is being updated.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool UpdatingPrivateKey
        {
            get { return updatingPrivateKey; }
            set { handler.Set(nameof(UpdatingPrivateKey), ref updatingPrivateKey, value); }
        }

        /// <summary>
        /// Command to run when updating the private key.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UpdatePrivateKeyCommand
        {
            get { return updatePrivateKeyCommand; }
            set { handler.Set(nameof(UpdatePrivateKeyCommand), ref updatePrivateKeyCommand, value); }
        }

        /// <summary>
        /// Validates the credentials entered by the user and saves them if they are valid.
        /// </summary>
        public void Save(WalmartStoreEntity store)
        {
            if (EnsureRequiredFieldsHaveValue())
            {
                store.ConsumerID = ConsumerID.Trim();
                store.ChannelType = ChannelType.Trim();

                if (!string.IsNullOrWhiteSpace(PrivateKey))
                {
                    store.PrivateKey = encryptionProvider.Encrypt(PrivateKey.Trim());
                }

                // Throws if credentials are not valid
                webClient.TestConnection(store);

                if (!IsNewStore)
                {
                    UpdatingPrivateKey = false;
                }
            }
        }

        /// <summary>
        /// Loads the credentials for the given store.
        /// </summary>
        public void Load(WalmartStoreEntity store)
        {
            ConsumerID = store.ConsumerID;
            ChannelType = store.ChannelType;

            IsNewStore = string.IsNullOrWhiteSpace(store.ConsumerID);
            if (IsNewStore)
            {
                UpdatingPrivateKey = true;
            }
        }

        /// <summary>
        /// Ensures the required fields have a value.
        /// </summary>
        private bool EnsureRequiredFieldsHaveValue()
        {
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(ConsumerID))
            {
                invalidFields.Add("Consumer ID");
            }

            if (string.IsNullOrWhiteSpace(ChannelType))
            {
                invalidFields.Add("Channel type");
            }

            if (string.IsNullOrWhiteSpace(PrivateKey) && IsNewStore)
            {
                invalidFields.Add("Private key");
            }

            if (invalidFields.Any())
            {
                throw new WalmartException($"Please enter your\n\t-{string.Join("\n\t-", invalidFields)}");
            }

            return !invalidFields.Any();
        }

        /// <summary>
        /// Called when [update private key].
        /// </summary>
        private void OnUpdatePrivateKey()
        {
            UpdatingPrivateKey = true;
        }
    }
}