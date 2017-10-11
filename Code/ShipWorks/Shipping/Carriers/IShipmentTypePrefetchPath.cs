using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Shipment type prefetch path collector
    /// </summary>
    public interface IShipmentTypePrefetchPath
    {
        /// <summary>
        /// Apply to a shipment query
        /// </summary>
        EntityQuery<ShipmentEntity> ApplyTo(EntityQuery<ShipmentEntity> query);

        /// <summary>
        /// Apply to an existing prefetch path
        /// </summary>
        IPrefetchPathElement2 ApplyTo(IPrefetchPathElement2 query);

        /// <summary>
        /// Apply to an arbitrary object using the apply method specified
        /// </summary>
        T ApplyTo<T>(T query, Func<T, IPrefetchPathElement2, T> applyMethod);

        /// <summary>
        /// Add a prefetch provider to this collector
        /// </summary>
        IShipmentTypePrefetchPath With(IShipmentTypePrefetchProvider x);
    }
}