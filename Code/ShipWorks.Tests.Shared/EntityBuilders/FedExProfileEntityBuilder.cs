using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a FedEx Profile entity
    /// </summary>
    public class FedExProfileEntityBuilder : EntityBuilder<FedExProfileEntity>
    {
        /// <summary>
        /// Add a package to the Profile
        /// </summary>
        public FedExProfileEntityBuilder WithPackage() => WithPackage(null);

        /// <summary>
        /// Add a package to the Profile
        /// </summary>
        public FedExProfileEntityBuilder WithPackage(Action<EntityBuilder<FedExProfilePackageEntity>> builderConfiguration)
        {
            EntityBuilder<FedExProfilePackageEntity> builder = new EntityBuilder<FedExProfilePackageEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Packages.Add(builder.Build()));

            return this;
        }
    }
}