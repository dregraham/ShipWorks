using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{

    /// <summary>
    /// Build a DHL shipment entity
    /// </summary>
    public class DhlExpressShipmentEntityBuilder : EntityBuilder<DhlExpressShipmentEntity>
    {
        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public DhlExpressShipmentEntityBuilder WithPackage() => WithPackage(null);

        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public DhlExpressShipmentEntityBuilder WithPackage(Action<EntityBuilder<DhlExpressPackageEntity>> builderConfiguration)
        {
            EntityBuilder<DhlExpressPackageEntity> builder = new EntityBuilder<DhlExpressPackageEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Packages.Add(builder.Build()));

            return this;
        }
    }
}