using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Viewmodel to set up an Amazon Order Source
    /// </summary>
    [Component]
    public class AmazonCreateOrderSourceViewModel : ViewModelBase, IAmazonCreateOrderSourceViewModel
    {
        private const string orderSourceName = "amazon";
        
        private readonly IWebHelper webHelper;
        private readonly IHubMonoauthClient hubMonoauthClient;
        private readonly IMessageHelper messageHelper;
        private bool openingUrl;
        private AmazonStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCreateOrderSourceViewModel(IWebHelper webHelper, IHubMonoauthClient hubMonoauthClient, IMessageHelper messageHelper)
        {
            this.webHelper = webHelper;
            this.hubMonoauthClient = hubMonoauthClient;
            this.messageHelper = messageHelper;

            GetOrderSourceId = new RelayCommand(async () => await GetOrderSourceIdCommand().ConfigureAwait(true));
        }

        /// <summary>
        /// Grabs a URL and opens up the browser to that page
        /// </summary>
        private async Task GetOrderSourceIdCommand()
        {
            OpeningUrl = true;
            try
            {
                var url = await hubMonoauthClient.GetCreateOrderSourceInitiateUrl(orderSourceName, store.AmazonApiRegion).ConfigureAwait(true);
                webHelper.OpenUrl(url);
            }
            catch (Exception ex)
            {
                messageHelper.ShowError(ex.GetBaseException().Message);
            }
            finally
            {
                OpeningUrl = false;
            }
        }

        /// <summary>
        /// Open the browser to get a token
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetOrderSourceId { get; }

        /// <summary>
        /// The EncodedOrderSource
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string EncodedOrderSource { get; set; }

        /// <summary>
        /// Are we currently trying to open the Url
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool OpeningUrl
        {
            get => openingUrl;
            private set => Set(ref openingUrl, value);
        }

        /// <summary>
        /// Load the order source
        /// </summary>
        public void Load(AmazonStoreEntity store)
        {
            this.store = store;
            EncodedOrderSource = Encode(store);
        }

        /// <summary>
        /// Save the order source
        /// </summary>
        public void Save(AmazonStoreEntity store)
        {
            if (!Decode(EncodedOrderSource, store))
            {
                Console.WriteLine("Failed to decode the encoded order source.");
            }
        }

        /// <summary>
        /// Encodes the Amazon store's data fields into a formatted base64 encoded value.
        /// </summary>
        private static string Encode(AmazonStoreEntity store)
        {
            var plainText = $"{store.MerchantID}_{store.MarketplaceID}_{store.OrderSourceID}";
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        /// <summary>
        /// Decodes the provided value and if successful populates the store's data fields.
        /// </summary>
        private static bool Decode(string encodedOrderSource, AmazonStoreEntity store)
        {
            try
            {
                var data = Convert.FromBase64String(encodedOrderSource);
                var decodedString = Encoding.UTF8.GetString(data);
                var splitString = decodedString.Split('_');

                if (splitString.Length != 3)
                {
                    Console.WriteLine("The provided Base64 string did not have 3 sections separated by the '_' character.");
                    return false;
                }

                store.MerchantID = splitString[0];
                store.MarketplaceID = splitString[1];
                store.OrderSourceID = splitString[2];

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}