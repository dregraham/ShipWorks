using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Used to capture or initialize ReturnItem data and associate it with a shipment.
    /// </summary>
    public class ReturnItemRepository : IReturnItemRepository
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnItemRepository(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Loads shipment with ShipmentReturnItems.
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="createIfNone">if set to <c>true</c> [create the return item if it does not exist].</param>
        public void LoadReturnData(ShipmentEntity shipment, bool createIfNone)
        {
            if (shipment.ShipmentReturnItem.None())
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    EntityQuery<ShipmentReturnItemEntity> query = new QueryFactory().ShipmentReturnItem
                        .Where(ShipmentReturnItemFields.ShipmentID == shipment.ShipmentID);

                    IEnumerable<ShipmentReturnItemEntity> returnItems =
                        sqlAdapter.FetchQueryAsync(query).Result.Cast<ShipmentReturnItemEntity>();

                    shipment.ShipmentReturnItem.AddRange(returnItems);

                    if (shipment.ShipmentReturnItem.None() && createIfNone)
                    {
                        InitializeReturnData(shipment, sqlAdapter);
                    }
                }
            }
        }

        /// <summary>
        /// Populates shipments with ShipmentReturnItems based on the order.
        /// </summary>
        private void InitializeReturnData(ShipmentEntity shipment, ISqlAdapter sqlAdapter)
        {
            DynamicQuery<ShipmentReturnItemEntity> query = new QueryFactory().OrderItem
                .Select(() => new ShipmentReturnItemEntity
                {
                    Name = OrderItemFields.Name.ToValue<string>(),
                    Quantity = OrderItemFields.Quantity.ToValue<double>(),
                    Weight = OrderItemFields.Weight.ToValue<double>(),
                    SKU = OrderItemFields.SKU.ToValue<string>(),
                    Code = OrderItemFields.Code.ToValue<string>(),
                    Notes = string.Empty
                }).Where(OrderItemFields.OrderID == shipment.OrderID);

            shipment.ShipmentReturnItem.AddRange(sqlAdapter.FetchQuery(query));
        }
    }
}