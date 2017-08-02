using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Jet downloader
    /// </summary>
    [KeyedComponent(typeof(StoreDownloader), StoreTypeCode.Jet, ExternallyOwned = true)]
    public class JetDownloader : StoreDownloader
    {
        private readonly IJetOrderLoader orderLoader;
        private readonly IJetWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetDownloader(StoreEntity store, Func<IOrderElementFactory, IJetOrderLoader> orderLoaderFactory, IJetWebClient webClient) : base(store)
        {
            orderLoader = orderLoaderFactory(this);
            this.webClient = webClient;
        }

        /// <summary>
        /// Download orders from jet
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                IEnumerable<JetOrderDetailsResult> orders = GetOrders();

                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                while (orders.Any())
                {
                    foreach (JetOrderDetailsResult jetOrder in orders)
                    {
                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        // Update the status
                        Progress.Detail = $"Processing order {QuantitySaved + 1}...";

                        LoadAndAcknowledgeOrder(jetOrder);

                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }
                    }

                    orders = GetOrders();
                }
            }
            catch (JetException ex)
            {
                throw new DownloadException(ex.Message);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }

            Progress.PercentComplete = 100;
            Progress.Detail = "Done";
        }

        /// <summary>
        /// Get orders from the web client, if something goes wrong throw a download exception
        /// </summary>
        private List<JetOrderDetailsResult> GetOrders()
        {
            GenericResult<IEnumerable<JetOrderDetailsResult>> ordersResult = webClient.GetOrders();

            if (ordersResult.Failure)
            {
                throw new DownloadException($"An error occured while downloading from Jet.com: {ordersResult.Message}");
            }

            return ordersResult.Value.ToList();
        }

        /// <summary>
        /// Load and acknowledge the order
        /// </summary>
        /// <param name="jetOrder"></param>
        private void LoadAndAcknowledgeOrder(JetOrderDetailsResult jetOrder)
        {
            JetOrderEntity order =
                InstantiateOrder(new GenericOrderIdentifier(jetOrder.ReferenceOrderId)) as JetOrderEntity;

            orderLoader.LoadOrder(order, jetOrder);

            SaveDownloadedOrder(order);
            webClient.Acknowledge(order);
        }
    }
}