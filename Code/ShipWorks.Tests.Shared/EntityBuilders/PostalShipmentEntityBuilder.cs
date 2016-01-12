using System;
using System.Linq.Expressions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a USPS shipment entity
    /// </summary>
    public class PostalShipmentEntityBuilder : EntityBuilder<PostalShipmentEntity>
    {
        private readonly ShipmentEntityBuilder parentBuilder;

        public PostalShipmentEntityBuilder(ShipmentEntityBuilder builder)
        {
            parentBuilder = builder;
        }

        /// <summary>
        /// Make the shipment a USPS shipment
        /// </summary>
        public PostalShipmentEntityBuilder AsUsps() => AsUsps(null);

        /// <summary>
        /// Make the shipment a USPS shipment
        /// </summary>
        public PostalShipmentEntityBuilder AsUsps(Action<EntityBuilder<UspsShipmentEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.Usps, x => x.Usps);

        /// <summary>
        /// Make the shipment an Endicia shipment
        /// </summary>
        public PostalShipmentEntityBuilder AsEndicia() => AsEndicia(null);

        /// <summary>
        /// Make the shipment an Endicia shipment
        /// </summary>
        public PostalShipmentEntityBuilder AsEndicia(Action<EntityBuilder<EndiciaShipmentEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.Endicia, x => x.Endicia);

        /// <summary>
        /// Set the shipment type
        /// </summary>
        private PostalShipmentEntityBuilder SetShipmentType<T, TBuilder>(Action<TBuilder> builderConfiguration,
            ShipmentTypeCode shipmentTypeCode,
            Expression<Func<PostalShipmentEntity, T>> shipmentAccessor)
            where T : EntityBase2, new()
            where TBuilder : EntityBuilder<T>, new()
        {
            TBuilder builder = new TBuilder();
            builderConfiguration?.Invoke(builder);

            parentBuilder.Set(x => x.ShipmentTypeCode, shipmentTypeCode);
            Set(shipmentAccessor, builder.Build());

            return this;
        }
    }
}