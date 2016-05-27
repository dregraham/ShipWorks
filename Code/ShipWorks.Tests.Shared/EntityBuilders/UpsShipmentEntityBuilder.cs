using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{

    /// <summary>
    /// Build a Ups shipment entity
    /// </summary>
    public class UpsShipmentEntityBuilder : EntityBuilder<UpsShipmentEntity>
    {
        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public UpsShipmentEntityBuilder WithPackage() => WithPackage(null);

        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public UpsShipmentEntityBuilder WithPackage(Action<EntityBuilder<UpsPackageEntity>> builderConfiguration)
        {
            EntityBuilder<UpsPackageEntity> builder = new EntityBuilder<UpsPackageEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Packages.Add(builder.Build()));

            return this;
        }
    }
}