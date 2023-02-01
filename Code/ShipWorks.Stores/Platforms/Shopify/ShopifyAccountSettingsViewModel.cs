using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Viewmodel to initiate the updating of a user's Shopify Credentials
    /// </summary>
    [Component]
    public class ShopifyAccountSettingsViewModel : ViewModelBase, IShopifyAccountSettingsViewModel
    {
        private const string orderSourceName = "shopify";

        private IShopifyStoreEntity store;
        private bool openingUrl;
        private readonly IWebHelper webHelper;
        private readonly IHubOrderSourceClient hubOrderSourceClient;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAccountSettingsViewModel(IWebHelper webHelper, IHubOrderSourceClient hubOrderSourceClient, IMessageHelper messageHelper)
        {
            this.webHelper = webHelper;
            this.hubOrderSourceClient = hubOrderSourceClient;
            this.messageHelper = messageHelper;

            UpdateOrderSource = new RelayCommand(async () => await UpdateOrderSourceCommand().ConfigureAwait(true));
        }

        /// <summary>
        /// Open the browser update the order source credentials
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand UpdateOrderSource { get; }

        /// <summary>
        /// Load the shopify store into the control
        /// </summary>
        public void Load(IShopifyStoreEntity shopifyStoreEntity)
        {
            store = shopifyStoreEntity;
        }

        /// <summary>
        /// Open the browser update the order source credentials
        /// </summary>
        private async Task UpdateOrderSourceCommand()
        {
            OpeningUrl = true;
            try
            {
                var url = await hubOrderSourceClient.GetUpdateOrderSourceInitiateUrl(orderSourceName, store.OrderSourceID).ConfigureAwait(true);
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
        /// Are we currently trying to open the Url
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool OpeningUrl
        {
            get => openingUrl;
            private set => Set(ref openingUrl, value);
        }
    }
}
