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
        private readonly IHubOrderSourceClient hubOrderSourceClient;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private bool openingUrl;
        private AmazonStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCreateOrderSourceViewModel(IWebHelper webHelper, IHubOrderSourceClient hubOrderSourceClient, IMessageHelper messageHelper, Func<Type, ILog> logFactory)
        {
            this.webHelper = webHelper;
            this.hubOrderSourceClient = hubOrderSourceClient;
            this.messageHelper = messageHelper;
            log = logFactory(typeof(AmazonCreateOrderSourceViewModel));

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
                var url = await hubOrderSourceClient.GetCreateOrderSourceInitiateUrl(orderSourceName, store.AmazonApiRegion, store.InitialDownloadDays).ConfigureAwait(true);
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
        public void Load(AmazonStoreEntity store)
        {
            this.store = store;
            EncodedOrderSource = Encode(store);
        }

        /// <summary>
        /// Save the order source
        /// </summary>
        public bool Save(AmazonStoreEntity store)
        {
            if (!Decode(EncodedOrderSource, store))
            {
                log.Error("Failed to decode the encoded order source token.");
                messageHelper.ShowError("The token does not appear to be correct. Please copy and paste it again.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Encodes the Amazon store's data fields into a formatted base64 encoded value.
        /// </summary>
        private static string Encode(AmazonStoreEntity store)
        {
            if (store.OrderSourceID.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            var plainText = $"{store.MerchantID}_{store.MarketplaceID}_{store.OrderSourceID}";
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        /// <summary>
        /// Decodes the provided value and if successful populates the store's data fields.
        /// </summary>
        private bool Decode(string encodedOrderSource, AmazonStoreEntity store)
        {
            try
            {
                var data = Convert.FromBase64String(encodedOrderSource);
                var decodedString = Encoding.UTF8.GetString(data);
                var splitString = decodedString.Split('_');

                if (splitString.Length != 3)
                {
                    log.Error("The provided Base64 string did not have 3 sections separated by the '_' character.");
                    return false;
                }

                if (!GuidHelper.IsGuid(splitString[2]))
                {
                    log.Error("The provided token's third part was not a GUID");
                    return false;
                }

                store.MerchantID = splitString[0];
                store.MarketplaceID = splitString[1];
                store.OrderSourceID = splitString[2];

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