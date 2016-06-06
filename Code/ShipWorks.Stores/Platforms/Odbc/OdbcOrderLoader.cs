using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Responsible for loading data into the OrderEntity
    /// </summary>
    class OdbcOrderLoader
    {
        private readonly IEnumerable<IOdbcOrderDetailLoader> orderDetailLoaders;
        private readonly IOdbcOrderItemLoader orderItemLoader;

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
            // load the items into the order
            orderItemLoader.Load(map, order, records);

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
        }
    }
}
