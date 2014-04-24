﻿using System.Threading.Tasks;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Users.Audit;

namespace ShipWorks.Shipping.ShipSense.Population
{
    /// <summary>
    /// Loads the ShipSenseKnowledgebase based on shipment history
    /// </summary>
    public class ShipSenseLoader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipSenseLoader));
        private readonly IShipSenseLoaderGateway shipSenseLoaderGateway;
        private static object runningLock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseLoader(IShipSenseLoaderGateway shipSenseLoaderGateway)
        {
            this.shipSenseLoaderGateway = shipSenseLoaderGateway;
        }

        /// <summary>
        /// Starts loading order and ship sense data on a new thread.
        /// </summary>
        public static void StartLoading()
        {
            using (ShipSenseLoaderGateway gateway = new ShipSenseLoaderGateway(new Knowledgebase()))
            {
                ShipSenseLoader shipSenseLoader = new ShipSenseLoader(gateway);
                Task.Factory.StartNew(shipSenseLoader.LoadData);
            }
        }

        /// <summary>
        /// Populates the ShipSenseKnowledgebase based on shipment history
        /// </summary>
        public void LoadData()
        {
            lock (runningLock)
            {
                UpdateOrderHashes();
            }
        }

        /// <summary>
        /// Populates the ShipSenseHashKey of orders that have a blank ShipSenseHashKey
        /// </summary>
        private void UpdateOrderHashes()
        {
            string appLockName = "ShipSenseLoader_Working";

            if (shipSenseLoaderGateway.GetAppLock(appLockName))
            {
                try
                {
                    using (new LoggedStopwatch(log, "ShipSenseLoader.UpdateOrderHashes"))
                    {
                        OrderEntity order = shipSenseLoaderGateway.FetchNextOrderOrderToProcess();
                        while (order != null)
                        {
                            OrderUtility.UpdateShipSenseHashKey(order);
                            shipSenseLoaderGateway.SaveOrder(order);

                            order = shipSenseLoaderGateway.FetchNextOrderOrderToProcess(order);
                        }
                    }

                    // Now populate any kb entries
                    AddKnowledgebaseEntries();
                }
                finally
                {
                    shipSenseLoaderGateway.ReleaseAppLock(appLockName);
                }
            }
            else
            {
                log.DebugFormat("Unable to get app lock ShipSenseLoader_Working");
            }
        }

        /// <summary>
        /// Adds a knowledgebase entry for each unique order that has processed shipments
        /// </summary>
        private void AddKnowledgebaseEntries()
        {
            using (new LoggedStopwatch(log, "ShipSenseLoader.AddKnowledgebaseEntries"))
            {
                using (new AuditBehaviorScope(AuditState.Disabled))
                {
                    ShipmentEntity shipment = shipSenseLoaderGateway.FetchNextShipmentToProcess();
                    while (shipment != null)
                    {
                        ShippingManager.EnsureShipmentLoaded(shipment);
                        shipSenseLoaderGateway.Save(shipment);

                        shipment = shipSenseLoaderGateway.FetchNextShipmentToProcess();
                    }
                }
            }
        }
    }
}
