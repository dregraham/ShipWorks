using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Shipping.Editing;
using ShipWorks.Filters;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Loads shipments from a list of orders in the background with progress
    /// </summary>
    public class ShipmentsLoader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentsLoader));

        Control owner;
        object tag;

        /// <summary>
        /// Raised when an asyn load as completed
        /// </summary>
        public event ShipmentsLoadedEventHandler LoadCompleted;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsLoader(Control owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        /// <summary>
        /// The maximum number of orders that we support loading at a time.
        /// </summary>
        public static int MaxAllowedOrders
        {
            get { return 1000; }
        }

        /// <summary>
        /// User defined data that can be associated with the loader
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        public void LoadAsync(IEnumerable<long> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }

            int count = keys.Count();

            if (count > MaxAllowedOrders)
            {
                throw new InvalidOperationException("Too many orders trying to load at once.");
            }

            EntityType keyType = count > 0 ? EntityUtility.GetEntityType(keys.First()) : EntityType.OrderEntity;

            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(owner,
                "Load Shipments",
                "ShipWorks is loading shipments for the selected orders.",
                "Loading {0} of {1}");

            // What to do before it gets started (but is on the background thread)
            executor.ExecuteStarting += (sender, args) =>
                {
                    // We need to make sure filters are up to date so profiles being applied can be as accurate as possible.
                    FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));
                };

            // What to do when its all done
            executor.ExecuteCompleted += new BackgroundExecutorCompletedEventHandler<long>(OnLoadShipmentsCompleted);

            // What to do for each shipment
            executor.ExecuteAsync((entityID, state, issueAdder) =>
            {
                List<ShipmentEntity> globalShipments = (List<ShipmentEntity>) state;

                try
                {
                    List<ShipmentEntity> iterationShipments = new List<ShipmentEntity>();

                    if (keyType == EntityType.OrderEntity)
                    {
                        bool createIfNone = UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, entityID);

                        iterationShipments.AddRange(ShippingManager.GetShipments(entityID, createIfNone));
                    }
                    else if (keyType == EntityType.ShipmentEntity)
                    {
                        ShipmentEntity shipment = ShippingManager.GetShipment(entityID);
                        if (shipment != null)
                        {
                            iterationShipments.Add(shipment);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid key type passed to load shipments.");
                    }

                    // Add them to the global list
                    globalShipments.AddRange(iterationShipments);
                }
                catch (SqlForeignKeyException)
                {
                    // If the order got deleted just forget it - its not an error, the shipments just don't load.
                    log.WarnFormat("Did not load shipments for entity {0} due to FK exception.", entityID);
                }
            },
            keys,
            new List<ShipmentEntity>());

        }

        /// <summary>
        /// The async loading of shipments for shipping has completed
        /// </summary>
        void OnLoadShipmentsCompleted(object sender, BackgroundExecutorCompletedEventArgs<long> e)
        {
            List<ShipmentEntity> shipments = (List<ShipmentEntity>) e.UserState;

            if (e.Canceled)
            {
                shipments.Clear();
            }

            ShipmentsLoadedEventHandler handler = LoadCompleted;
            if (handler != null)
            {
                ShipmentsLoadedEventArgs args = new ShipmentsLoadedEventArgs(null, e.Canceled, e.UserState, shipments);
                handler(this, args);
            }
        }
    }
}
