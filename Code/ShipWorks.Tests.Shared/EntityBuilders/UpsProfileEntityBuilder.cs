using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a Ups Profile entity
    /// </summary>
    public class UpsProfileEntityBuilder : EntityBuilder<UpsProfileEntity>
    {
        /// <summary>
        /// Add a package to the Profile
        /// </summary>
        public UpsProfileEntityBuilder WithPackage() => WithPackage(null);

        /// <summary>
        /// Add a package to the Profile
        /// </summary>
        public UpsProfileEntityBuilder WithPackage(Action<EntityBuilder<UpsProfilePackageEntity>> builderConfiguration)
        {
            EntityBuilder<UpsProfilePackageEntity> builder = new EntityBuilder<UpsProfilePackageEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Packages.Add(builder.Build()));

            return this;
        }
    }
}