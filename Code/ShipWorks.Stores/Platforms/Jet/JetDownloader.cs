using System;
using System.Collections.Generic;
using System.Linq;
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
                GenericResult<IEnumerable<JetOrderDetailsResult>> orders = webClient.GetOrders();

                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                while (orders.Success && orders.Value.Any())
                {
                    foreach (JetOrderDetailsResult jetOrder in orders.Value)
                    {
                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        // Update the status
                        Progress.Detail = $"Processing order {QuantitySaved + 1}...";

                        JetOrderEntity order =
                            InstantiateOrder(new GenericOrderIdentifier(jetOrder.ReferenceOrderId)) as JetOrderEntity;

                        orderLoader.LoadOrder(order, jetOrder);

                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        SaveDownloadedOrder(order);
                        webClient.Acknowledge(order);
                    }

                    // Check if it has been canceled
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    orders = webClient.GetOrders();
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
    }
}