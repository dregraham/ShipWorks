using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
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

        private readonly IWebHelper webHelper;
        private readonly IHubOrderSourceClient hubOrderSourceClient;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        private bool openingUrl;
        private ShopifyStoreEntity store;
        private string OrderSourceName => store.StoreTypeCode.ToString().ToLowerInvariant();

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
                var parameters = new Dictionary<string, string>();
                parameters.Add("shopify_domain", ShopifyHelper.GetShopUrl(ShopifyShopUrlName));
                parameters.Add("notify_buyer", store.ShopifyNotifyCustomer.ToString());

                var url = await hubOrderSourceClient.GetCreateOrderSourceInitiateUrl(OrderSourceName, store.InitialDownloadDays, parameters).ConfigureAwait(true);
                webHelper.OpenUrl(url);
            }
            catch (ObjectDisposedException ex)
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
        [Obfuscation(Exclude = true)]
        public string ShopifyShopUrlName { get; set; }

        /// <summary>
        /// Load the order source
        /// </summary>
        public void Load(ShopifyStoreEntity store)
        {
            this.store = store;
            EncodedOrderSource = Encode(store);
            ShopifyShopUrlName = store.ShopifyShopUrlName;
        }

        /// <summary>
        /// Save the order source
        /// </summary>
        public async virtual Task<bool> Save(ShopifyStoreEntity store)
        {
            if (string.IsNullOrWhiteSpace(ShopifyShopUrlName))
            {
                messageHelper.ShowError("The Shopify shop name is empty. Please enter correct Shopify address.");
                return false;
            }
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
                CreateOrderSourceResult createOrderSourceResult;
                try
                {
                    createOrderSourceResult = JsonConvert.DeserializeObject<CreateOrderSourceResult>(decodedString);
                }
                catch (Exception ex)
                {
                    log.Error("The provided string could not be deserialized.");
                    return false;
                }
                if (createOrderSourceResult.StoreType != "Shopify")
                {
                    log.Error("The provided token is not valid Shopify token.");
                    return false;
                }

                if (createOrderSourceResult.Domain != ShopifyHelper.GetShopUrl(ShopifyShopUrlName))
                {
                    log.Error($"The provided token is not valid for shop `{ShopifyShopUrlName}`. It is created for ${createOrderSourceResult.Domain}");
                    return false;
                }

                if (!GuidHelper.IsGuid(createOrderSourceResult.OrderSourceId))
                {
                    log.Error("The provided token's third part was not a GUID");
                    return false;
                }

                store.ShopifyShopUrlName = ShopifyShopUrlName;

                store.OrderSourceID = createOrderSourceResult.OrderSourceId;

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

