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
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Viewmodel to set up an Shopify Order Source
    /// </summary>
    [Component]
    public class ShopifyCreateOrderSourceViewModel : ViewModelBase, IShopifyCreateOrderSourceViewModel
    {
        private const string orderSourceName = "Shopify";

        private readonly IWebHelper webHelper;
        private readonly IHubOrderSourceClient hubOrderSourceClient;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private bool openingUrl;
        private ShopifyStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCreateOrderSourceViewModel(IWebHelper webHelper, IHubOrderSourceClient hubOrderSourceClient, IMessageHelper messageHelper, Func<Type, ILog> logFactory)
        {
            this.webHelper = webHelper;
            this.hubOrderSourceClient = hubOrderSourceClient;
            this.messageHelper = messageHelper;
            log = logFactory(typeof(ShopifyCreateOrderSourceViewModel));

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
                var url = await hubOrderSourceClient.GetCreateOrderSourceInitiateUrl(orderSourceName, store.InitialDownloadDays).ConfigureAwait(true);
                webHelper.OpenUrl(url);
            }
            catch(ObjectDisposedException ex)
            {
                //User cancelled the dialog before we could open the browser
                //Do nothing
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
        public void Load(ShopifyStoreEntity store)
        {
            this.store = store;
            EncodedOrderSource = Encode(store);
        }

        /// <summary>
        /// Save the order source
        /// </summary>
        public bool Save(ShopifyStoreEntity store)
        {
            if (string.IsNullOrWhiteSpace(EncodedOrderSource))
            {
                messageHelper.ShowError("The token is empty. Please copy and paste it.");
                return false;
            }
            else if (!Decode(EncodedOrderSource, store))
            {
                log.Error("Failed to decode the encoded order source token.");
                messageHelper.ShowError("The token does not appear to be correct. Please copy and paste it again.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Encodes the Shopify store's data fields into a formatted base64 encoded value.
        /// </summary>
        private static string Encode(ShopifyStoreEntity store)
        {
            if (store.OrderSourceID.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            var plainText = $"Shopify-{store.OrderSourceID}";
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        /// <summary>
        /// Decodes the provided value and if successful populates the store's data fields.
        /// </summary>
        private bool Decode(string encodedOrderSource, ShopifyStoreEntity store)
        {
            try
            {
                var data = Convert.FromBase64String(encodedOrderSource);
                var decodedString = Encoding.UTF8.GetString(data);
                var splitString = decodedString.Split('_');

                if (splitString.Length != 2)
                {
                    log.Error("The provided Base64 string did not have 2 sections separated by the '_' character.");
                    return false;
                }

                if (!GuidHelper.IsGuid(splitString[1]))
                {
                    log.Error("The provided token's third part was not a GUID");
                    return false;
                }
                
                var prefix = splitString[0].Split('-');
                if(prefix.Length != 2 || prefix[0]!= "Shopify")
                {
                    log.Error("The provided token is not valid Shopify token. It does not start with 'Shopify-' prefix");
                    return false;
                }

                var shopifyDomain = prefix[1];
                
                store.ShopifyShopUrlName = shopifyDomain;

                store.OrderSourceID = splitString[1];

                return true;
            }
            catch (Exception e)
            {
                log.Error(e);
                return false;
            }
        }
    }
}