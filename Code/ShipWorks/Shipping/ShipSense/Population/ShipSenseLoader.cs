using System.Threading.Tasks;
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
        private const string AppLockName = "ShipSenseLoader_Working";
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipSenseLoader));
        
        private readonly IShipSenseLoaderGateway shipSenseLoaderGateway;
        private static object runningLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseLoader"/> class.
        /// </summary>
        /// <param name="shipSenseLoaderGateway">The ship sense loader gateway.</param>
        public ShipSenseLoader(IShipSenseLoaderGateway shipSenseLoaderGateway)
        {
            this.shipSenseLoaderGateway = shipSenseLoaderGateway;
        }

        /// <summary>
        /// Starts loading order and ShipSense data on a new thread.
        /// </summary>
        public static void LoadDataAsync()
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
                if (shipSenseLoaderGateway.GetAppLock(AppLockName))
                {
                    try
                    {
                        // Re-calculate the ShipSense hash key for orders that are eligible
                        // and add entries to the ShipSense knowledge base for the orders
                        // that have processed shipment.
                        UpdateOrderHashes();
                        AddKnowledgebaseEntries();
                    }
                    finally
                    {
                        shipSenseLoaderGateway.ReleaseAppLock(AppLockName);
                    }
                }
                else
                {
                    log.DebugFormat("Unable to get applock ShipSenseLoader_Working");
                }
            }
        }

        /// <summary>
        /// Populates the ShipSenseHashKey of orders that have a blank ShipSenseHashKey
        /// </summary>
        private void UpdateOrderHashes()
        {
            OrderEntity order = shipSenseLoaderGateway.FetchNextOrderOrderToProcess();
            while (order != null)
            {
                OrderUtility.UpdateShipSenseHashKey(order);
                shipSenseLoaderGateway.SaveOrder(order);

                order = shipSenseLoaderGateway.FetchNextOrderOrderToProcess(order);
            }
        }

        /// <summary>
        /// Adds a knowledge base entry for each unique order that has processed shipments
        /// </summary>
        private void AddKnowledgebaseEntries()
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
