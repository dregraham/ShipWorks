using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Viewmodel to set up an Shopify Order Source
    /// </summary>
    [Component]
    public class ShopifyMigrateOrderSourceViewModel : ShopifyCreateOrderSourceViewModel, IShopifyMigrateOrderSourceViewModel
    {
        private readonly IHubOrderSourceClient hubOrderSourceClient;
        private readonly IMessageHelper messageHelper;
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyMigrateOrderSourceViewModel(IWebHelper webHelper, IHubOrderSourceClient hubOrderSourceClient, IMessageHelper messageHelper, Func<Type, ILog> logFactory) :
            base(webHelper, hubOrderSourceClient, messageHelper, logFactory)
        {
            this.hubOrderSourceClient = hubOrderSourceClient;
            this.messageHelper = messageHelper;
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Save the order source
        /// </summary>
        public override async Task<bool> Save(ShopifyStoreEntity store)
        {
            if (store.ShopifyShopUrlName != ShopifyShopUrlName)
            {
                messageHelper.ShowError("You cannot change url name!");
                return false;
            }
            if (!await base.Save(store))
            {
                return false;
            }
            try
            {
                if (store.WarehouseStoreID != null)
                {
                    await hubOrderSourceClient.MigrateStoreToPlatform(store.WarehouseStoreID.ToString(), store.OrderSourceID);
                }
                return true;
            }
            catch (Exception ex)
            {
                var logger = logFactory(typeof(ShopifyMigrateOrderSourceViewModel));

                logger.Error("An error occured updating the shopify store settings in platform", ex);
                messageHelper.ShowError("Failed to update settings. Please try again.");
                return false;
            }
        }
    }
}