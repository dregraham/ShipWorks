using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a FedEx shipment entity
    /// </summary>
    public class FedExShipmentEntityBuilder : EntityBuilder<FedExShipmentEntity>
    {
        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public FedExShipmentEntityBuilder WithPackage() => WithPackage(null);

        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public FedExShipmentEntityBuilder WithPackage(Action<EntityBuilder<FedExPackageEntity>> builderConfiguration)
        {
            EntityBuilder<FedExPackageEntity> builder = new EntityBuilder<FedExPackageEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Packages.Add(builder.Build()));

            return this;
        }
    }
}