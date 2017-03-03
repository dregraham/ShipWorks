using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    public class WalmartStoreSetupControlViewModel : IWalmartStoreSetupControlViewModel
    {
        private readonly IWalmartWebClient webClient;
        private readonly IMessageHelper messageHelper;
        private readonly IEncryptionProvider encryptionProvider;
        private string consumerID;
        private string privateKey;
        private string channelType;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartStoreSetupControlViewModel"/> class.
        /// </summary>
        public WalmartStoreSetupControlViewModel(IWalmartWebClient webClient, IMessageHelper messageHelper, 
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.webClient = webClient;
            this.messageHelper = messageHelper;
            encryptionProvider = encryptionProviderFactory.CreateWalmartEncryptionProvider();
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
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
        /// Validates the credentials entered by the user and saves them if they are valid.
        /// </summary>
        public bool Save(WalmartStoreEntity store)
        {
            bool validCredentials = false;
            List<string> invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(ConsumerID))
            {
                invalidFields.Add("Consumer ID");
            }

            if (string.IsNullOrWhiteSpace(PrivateKey))
            {
                invalidFields.Add("Private key");
            }

            if (string.IsNullOrWhiteSpace(ChannelType))
            {
                invalidFields.Add("Channel type");
            }

            if (invalidFields.Any())
            {
                messageHelper.ShowError($"Please enter your{string.Join("\n\t-", invalidFields)}");
            }

            store.ConsumerID = ConsumerID.Trim();
            store.PrivateKey = encryptionProvider.Encrypt(PrivateKey.Trim());
            store.ChannelType = ChannelType.Trim();

            try
            {
                webClient.TestConnection(store);

                // If test connection didn't throw, credentials were valid
                validCredentials = true;
            }
            catch (WalmartException ex)
            {
                messageHelper.ShowError($"Error connecting to Walmart: {ex.Message}");
            }

            return validCredentials;
        }
    }
}