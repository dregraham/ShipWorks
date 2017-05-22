﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    /// <summary>
    /// Responsible for loading data into the OrderEntity
    /// </summary>
    public class OdbcOrderLoader : IOdbcOrderLoader
    {
        private readonly IEnumerable<IOdbcOrderDetailLoader> orderDetailLoaders;
        private readonly IOdbcOrderItemLoader orderItemLoader;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IOrderChargeCalculator orderChargeCalculator;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcOrderLoader(IEnumerable<IOdbcOrderDetailLoader> orderDetailLoaders,
            IOdbcOrderItemLoader orderItemLoader,
            IDateTimeProvider dateTimeProvider,
            IOrderChargeCalculator orderChargeCalculator)
        {
            this.orderDetailLoaders = orderDetailLoaders;
            this.orderItemLoader = orderItemLoader;
            this.dateTimeProvider = dateTimeProvider;
            this.orderChargeCalculator = orderChargeCalculator;
        }

        /// <summary>
        /// Loads order information into the OrderEntity
        /// </summary>
        /// <remarks>
        /// It is assumed that the map is loaded with the initial record.
        /// </remarks>
        public void Load(IOdbcFieldMap map, OrderEntity order, IEnumerable<OdbcRecord> records)
        {
            MethodConditions.EnsureArgumentIsNotNull(map, nameof(map));
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            // Load the first record into the map
            map.CopyToEntity(order);

            // If the order is new load all the items
            if (order.IsNew)
            {
                // load the items into the order
                orderItemLoader.Load(map, order, records);

                // Clean up the order date, if it was never mapped the value is one
                // that cannot be inserted into SQL Server so make it UtcNow
                // this has to be done before we load order details as some order
                // detail items rely on the order date to be correct like Notes
                if (!order.Fields[(int)OrderFieldIndex.OrderDate].IsChanged)
                {
                    order.OrderDate = dateTimeProvider.UtcNow;
                }
            }

            foreach (IOdbcOrderDetailLoader loader in orderDetailLoaders)
            {
                // Load the map data into the order for the given
                // order detail loaders e.g. notes, charges, payments
                loader.Load(map, order);
            }

            // Now that all of the order information has been set calculate the total
            if (order.IsNew)
            {
                order.OrderTotal = orderChargeCalculator.CalculateTotal(order);
            }
        }
    }
}
