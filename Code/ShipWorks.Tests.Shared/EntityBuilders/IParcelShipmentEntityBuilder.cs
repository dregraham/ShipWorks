using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a IParcel shipment entity
    /// </summary>
    public class IParcelShipmentEntityBuilder : EntityBuilder<IParcelShipmentEntity>
    {
        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public IParcelShipmentEntityBuilder WithPackage() => WithPackage(null);

        /// <summary>
        /// Add a package to the shipment
        /// </summary>
        public IParcelShipmentEntityBuilder WithPackage(Action<EntityBuilder<IParcelPackageEntity>> builderConfiguration)
        {
            EntityBuilder<IParcelPackageEntity> builder = new EntityBuilder<IParcelPackageEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Packages.Add(builder.Build()));

            return this;
        }
    }
}