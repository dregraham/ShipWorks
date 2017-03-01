using System.ComponentModel;
using System.Reflection;
using ShipWorks.Core.UI;
using ShipWorks.Stores.Platforms.Walmart;

namespace ShipWorks.Stores.UI.Platforms.Walmart.WizardPages
{
    public class WalmartStoreSetupControlViewModel : IWalmartStoreSetupControlViewModel
    {
        private readonly IWalmartWebClient webClient;
        private string consumerID;
        private string privateKey;
        private string channelType;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        public WalmartStoreSetupControlViewModel(IWalmartWebClient webClient)
        {
            this.webClient = webClient;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        [Obfuscation(Exclude = true)]
        public string ConsumerID
        {
            get { return consumerID; }
            set { handler.Set(nameof(ConsumerID), ref consumerID, value); }
        }

        [Obfuscation(Exclude = true)]
        public string PrivateKey
        {
            get { return privateKey; }
            set { handler.Set(nameof(PrivateKey), ref privateKey, value); }
        }

        [Obfuscation(Exclude = true)]
        public string ChannelType
        {
            get { return channelType; }
            set { handler.Set(nameof(ChannelType), ref channelType, value); }
        }
    }
}