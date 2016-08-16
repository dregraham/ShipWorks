using System;
using System.Linq.Expressions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a USPS Profile entity
    /// </summary>
    public class PostalProfileEntityBuilder : EntityBuilder<PostalProfileEntity>
    {
        private readonly ProfileEntityBuilder parentBuilder;
        private readonly bool isPrimaryProfile;

        public PostalProfileEntityBuilder(ProfileEntityBuilder builder, bool isPrimary)
        {
            parentBuilder = builder;
            isPrimaryProfile = isPrimary;
        }

        /// <summary>
        /// Make the Profile a USPS Profile
        /// </summary>
        public PostalProfileEntityBuilder AsUsps() => AsUsps(null);

        /// <summary>
        /// Make the Profile a USPS Profile
        /// </summary>
        public PostalProfileEntityBuilder AsUsps(Action<EntityBuilder<UspsProfileEntity>> builderConfiguration) =>
            SetProfileType(builderConfiguration, ShipmentTypeCode.Usps, x => x.Usps);

        /// <summary>
        /// Make the Profile an Endicia Profile
        /// </summary>
        public PostalProfileEntityBuilder AsEndicia() => AsEndicia(null);

        /// <summary>
        /// Make the Profile an Endicia Profile
        /// </summary>
        public PostalProfileEntityBuilder AsEndicia(Action<EntityBuilder<EndiciaProfileEntity>> builderConfiguration) =>
            SetProfileType(builderConfiguration, ShipmentTypeCode.Endicia, x => x.Endicia);

        /// <summary>
        /// Set the Profile type
        /// </summary>
        private PostalProfileEntityBuilder SetProfileType<T, TBuilder>(Action<TBuilder> builderConfiguration,
            ShipmentTypeCode ProfileTypeCode,
            Expression<Func<PostalProfileEntity, T>> ProfileAccessor)
            where T : EntityBase2, new()
            where TBuilder : EntityBuilder<T>, new()
        {
            TBuilder builder = new TBuilder();
            builderConfiguration?.Invoke(builder);

            if (isPrimaryProfile)
            {
                builder.SetDefaultsOnNullableFields();
            }

            parentBuilder.Set(x => x.ShipmentTypeCode, ProfileTypeCode);
            Set(ProfileAccessor, builder.Build());

            return this;
        }
    }
}