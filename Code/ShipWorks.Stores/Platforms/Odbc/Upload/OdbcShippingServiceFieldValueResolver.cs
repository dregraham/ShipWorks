using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
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
            ShipmentEntity shipment = entity as ShipmentEntity;

            return shipment == null ? null : shippingManager.GetServiceUsed(shipment);
        }
    }
}