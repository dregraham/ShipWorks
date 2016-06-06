using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Responsible for loading data into the OrderEntity
    /// </summary>
    public class OdbcOrderLoader : IOdbcOrderLoader
    {
        private readonly IEnumerable<IOdbcOrderDetailLoader> orderDetailLoaders;
        private readonly IOdbcOrderItemLoader orderItemLoader;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcOrderLoader(IEnumerable<IOdbcOrderDetailLoader> orderDetailLoaders, IOdbcOrderItemLoader orderItemLoader)
        {
            this.orderDetailLoaders = orderDetailLoaders;
            this.orderItemLoader = orderItemLoader;
        }

        /// <summary>
        /// Loads order information into the OrderEntity
        /// </summary>
        /// <param name="map">the map</param>
        /// <param name="order">the order entity to populate</param>
        /// <param name="records">a collection of ODBC Records related to the order</param>
        public void Load(IOdbcFieldMap map, OrderEntity order, IEnumerable<OdbcRecord> records)
        {
            MethodConditions.EnsureArgumentIsNotNull(map, "map");
            MethodConditions.EnsureArgumentIsNotNull(order, "order");

            // Load the first record into the map
            map.ApplyValues(records.FirstOrDefault());
            map.CopyToEntity(order);

            foreach (OdbcRecord record in records)
            {
                // Apply the values from the record into the map
                map.ApplyValues(record);

                foreach (IOdbcOrderDetailLoader loader in orderDetailLoaders)
                {
                    // Load the map data into the order for the given
                    // order detail loaders e.g. notes, charges, payments
                    loader.Load(map, order);
                }
            }

            // load the items into the order
            orderItemLoader.Load(map, order, records);

            // This stuff is just to ensure that ShipWorks does not crash, it might need to change in the future
            if (order.OrderDate < DateTime.Parse("1/1/1753 12:00:00 AM"))
            {
                order.OrderDate = DateTime.UtcNow;
            }

            if (order.OnlineLastModified < DateTime.Parse("1/1/1753 12:00:00 AM"))
            {
                order.OnlineLastModified = DateTime.UtcNow;
            }

            if (order.IsNew)
            {
                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }
        }
    }
}
