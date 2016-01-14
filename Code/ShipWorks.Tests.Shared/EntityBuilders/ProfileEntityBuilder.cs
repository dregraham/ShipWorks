using System;
using System.Linq.Expressions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a profile entity
    /// </summary>
    public class ProfileEntityBuilder : EntityBuilder<ShippingProfileEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileEntityBuilder(ShippingProfileEntity profile) : base(profile)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileEntityBuilder()
        {
            Set(x => x.ShipmentTypeCode, ShipmentTypeCode.None);
        }

        /// <summary>
        /// Make the shipment a FedEx shipment
        /// </summary>
        public ProfileEntityBuilder AsFedEx() => AsFedEx(null);

        /// <summary>
        /// Sets this profile as the primary for the shipment type
        /// </summary>
        public ProfileEntityBuilder AsPrimary()
        {
            Set(x => x.ShipmentTypePrimary, true);
            return this;
        }

        /// <summary>
        /// Make the shipment a FedEx shipment
        /// </summary>
        public ProfileEntityBuilder AsFedEx(Action<FedExProfileEntityBuilder> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.FedEx, x => x.FedEx);

        /// <summary>
        /// Make the shipment a Ups shipment
        /// </summary>
        public ProfileEntityBuilder AsUps() => AsUps(null);

        /// <summary>
        /// Make the shipment a Ups shipment
        /// </summary>
        public ProfileEntityBuilder AsUps(Action<UpsProfileEntityBuilder> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.UpsOnLineTools, x => x.Ups);

        /// <summary>
        /// Make the shipment a Postal shipment
        /// </summary>
        public ProfileEntityBuilder AsPostal() => AsPostal(null);

        /// <summary>
        /// Make the shipment a Postal shipment
        /// </summary>
        public ProfileEntityBuilder AsPostal(Action<PostalProfileEntityBuilder> builderConfiguration)
        {
            Set(x => x.ShipmentTypeCode, ShipmentTypeCode.PostalWebTools);

            PostalProfileEntityBuilder builder = new PostalProfileEntityBuilder(this);
            builderConfiguration?.Invoke(builder);

            Set(x => x.Postal, builder.Build());

            return this;
        }

        /// <summary>
        /// Make the shipment an Amazon shipment
        /// </summary>
        public ProfileEntityBuilder AsAmazon() => AsAmazon(null);

        /// <summary>
        /// Make the shipment an Amazon shipment
        /// </summary>
        public ProfileEntityBuilder AsAmazon(Action<EntityBuilder<AmazonProfileEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.Amazon, x => x.Amazon);

        /// <summary>
        /// Make the shipment an BestRate shipment
        /// </summary>
        public ProfileEntityBuilder AsBestRate() => AsBestRate(null);

        /// <summary>
        /// Make the shipment an BestRate shipment
        /// </summary>
        public ProfileEntityBuilder AsBestRate(Action<EntityBuilder<BestRateProfileEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.BestRate, x => x.BestRate);

        /// <summary>
        /// Make the shipment an Other shipment
        /// </summary>
        public ProfileEntityBuilder AsOther() => AsOther(null);

        /// <summary>
        /// Make the shipment an Other shipment
        /// </summary>
        public ProfileEntityBuilder AsOther(Action<EntityBuilder<OtherProfileEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.Other, x => x.Other);

        /// <summary>
        /// Make the shipment an OnTrac shipment
        /// </summary>
        public ProfileEntityBuilder AsOnTrac() => AsOnTrac(null);

        /// <summary>
        /// Make the shipment an OnTrac shipment
        /// </summary>
        public ProfileEntityBuilder AsOnTrac(Action<EntityBuilder<OnTracProfileEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.OnTrac, x => x.OnTrac);

        /// <summary>
        /// Make the shipment an iParcel shipment
        /// </summary>
        public ProfileEntityBuilder AsIParcel() => AsIParcel(null);

        /// <summary>
        /// Make the shipment an iParcel shipment
        /// </summary>
        public ProfileEntityBuilder AsIParcel(Action<IParcelProfileEntityBuilder> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.iParcel, x => x.IParcel);

        /// <summary>
        /// Set the shipment type
        /// </summary>
        private ProfileEntityBuilder SetShipmentType<T, TBuilder>(Action<TBuilder> builderConfiguration,
            ShipmentTypeCode shipmentTypeCode,
            Expression<Func<ShippingProfileEntity, T>> shipmentAccessor)
            where T : EntityBase2, new()
            where TBuilder : EntityBuilder<T>, new()
        {
            TBuilder builder = new TBuilder();
            builderConfiguration?.Invoke(builder);

            Set(x => x.ShipmentTypeCode, shipmentTypeCode);
            Set(shipmentAccessor, builder.Build());

            return this;
        }

        public override ShippingProfileEntity Save(SqlAdapter adapter)
        {
            ShippingProfileEntity value = base.Save(adapter);

            // Make sure the new profile is seen by the store manager
            ShippingProfileManager.CheckForChangesNeeded();

            return value;
        }
    }
}