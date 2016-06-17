using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a IParcel Profile entity
    /// </summary>
    public class IParcelProfileEntityBuilder : EntityBuilder<IParcelProfileEntity>
    {
        /// <summary>
        /// Add a package to the Profile
        /// </summary>
        public IParcelProfileEntityBuilder WithPackage() => WithPackage(null);

        /// <summary>
        /// Add a package to the Profile
        /// </summary>
        public IParcelProfileEntityBuilder WithPackage(Action<EntityBuilder<IParcelProfilePackageEntity>> builderConfiguration)
        {
            EntityBuilder<IParcelProfilePackageEntity> builder = new EntityBuilder<IParcelProfilePackageEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Packages.Add(builder.Build()));

            return this;
        }
    }
}