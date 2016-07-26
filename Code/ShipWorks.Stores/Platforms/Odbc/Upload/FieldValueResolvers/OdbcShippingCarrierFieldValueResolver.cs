using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers
{
    /// <summary>
    /// Resolver for the carrier name
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers.IOdbcFieldValueResolver" />
    public class OdbcShippingCarrierFieldValueResolver : IOdbcFieldValueResolver
    {
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcShippingCarrierFieldValueResolver"/> class.
        /// </summary>
        /// <param name="shippingManager">The shipping manager.</param>
        public OdbcShippingCarrierFieldValueResolver(IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Get the carrier name if this is a shipment entity.
        /// </summary>
        public object GetValue(IShipWorksOdbcMappableField field, IEntity2 entity)
        {
            // Suppressing because, for this implementation, we only care about shipments. In other cases,
            // we care use the interface "properly."
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            ShipmentEntity shipment = entity as ShipmentEntity;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

            if (shipment==null)
            {
                return null;
            }

            return shippingManager.GetCarrierName(shipment.ShipmentTypeCode);
        }
    }
}