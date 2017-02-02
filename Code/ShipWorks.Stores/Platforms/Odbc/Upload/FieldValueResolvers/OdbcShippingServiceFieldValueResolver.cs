using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers
{
    /// <summary>
    /// Resolver for the Shipping Service
    /// </summary>
    public class OdbcShippingServiceFieldValueResolver : IOdbcFieldValueResolver
    {
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcShippingServiceFieldValueResolver(IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Gets the service used if the given entity is a shipmententity
        /// </summary>
        public object GetValue(IShipWorksOdbcMappableField field, IEntity2 entity)
        {
            // Suppressing because, for this implementation, we only care about shipments. In other cases,
            // we care use the interface "properly."
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            ShipmentEntity shipment = entity as ShipmentEntity;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

            return shipment == null ? null : shippingManager.GetOverriddenServiceUsed(shipment);
        }
    }
}