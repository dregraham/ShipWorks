using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a shipment entity
    /// </summary>
    public class ShipmentEntityBuilder : EntityBuilder<ShipmentEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentEntityBuilder(ShipmentEntity shipment) : base(shipment)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentEntityBuilder()
        {
            Set(x => x.ShipSenseChangeSets, new XElement("ChangeSets").ToString());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentEntityBuilder(OrderEntity order) : this()
        {
            Set(x => x.Order, order);
        }

        /// <summary>
        /// Make the shipment a FedEx shipment
        /// </summary>
        public ShipmentEntityBuilder AsFedEx() => AsFedEx(null);

        /// <summary>
        /// Make the shipment a FedEx shipment
        /// </summary>
        public ShipmentEntityBuilder AsFedEx(Action<FedExShipmentEntityBuilder> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.FedEx, x => x.FedEx);

        /// <summary>
        /// Make the shipment a Ups shipment
        /// </summary>
        public ShipmentEntityBuilder AsUps() => AsUps(null);

        /// <summary>
        /// Make the shipment a Ups shipment
        /// </summary>
        public ShipmentEntityBuilder AsUps(Action<UpsShipmentEntityBuilder> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.UpsOnLineTools, x => x.Ups);

        /// <summary>
        /// Make the shipment a Postal shipment
        /// </summary>
        public ShipmentEntityBuilder AsPostal() => AsPostal(null);

        /// <summary>
        /// Make the shipment a Postal shipment
        /// </summary>
        public ShipmentEntityBuilder AsPostal(Action<PostalShipmentEntityBuilder> builderConfiguration)
        {
            PostalShipmentEntityBuilder builder = new PostalShipmentEntityBuilder(this);
            builderConfiguration?.Invoke(builder);

            Set(x => x.ShipmentTypeCode, ShipmentTypeCode.PostalWebTools);
            Set(x => x.Postal, builder.Build());

            return this;
        }

        /// <summary>
        /// Make the shipment an Amazon shipment
        /// </summary>
        public ShipmentEntityBuilder AsAmazon() => AsAmazon(null);

        /// <summary>
        /// Make the shipment an Amazon shipment
        /// </summary>
        public ShipmentEntityBuilder AsAmazon(Action<EntityBuilder<AmazonShipmentEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.Amazon, x => x.Amazon);

        /// <summary>
        /// Make the shipment an BestRate shipment
        /// </summary>
        public ShipmentEntityBuilder AsBestRate() => AsBestRate(null);

        /// <summary>
        /// Make the shipment an BestRate shipment
        /// </summary>
        public ShipmentEntityBuilder AsBestRate(Action<EntityBuilder<BestRateShipmentEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.BestRate, x => x.BestRate);

        /// <summary>
        /// Make the shipment an Other shipment
        /// </summary>
        public ShipmentEntityBuilder AsOther() => AsOther(null);

        /// <summary>
        /// Make the shipment an Other shipment
        /// </summary>
        public ShipmentEntityBuilder AsOther(Action<EntityBuilder<OtherShipmentEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.Other, x => x.Other);

        /// <summary>
        /// Make the shipment an OnTrac shipment
        /// </summary>
        public ShipmentEntityBuilder AsOnTrac() => AsOnTrac(null);

        /// <summary>
        /// Make the shipment an OnTrac shipment
        /// </summary>
        public ShipmentEntityBuilder AsOnTrac(Action<EntityBuilder<OnTracShipmentEntity>> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.OnTrac, x => x.OnTrac);

        /// <summary>
        /// Make the shipment an iParcel shipment
        /// </summary>
        public ShipmentEntityBuilder AsIParcel() => AsIParcel(null);

        /// <summary>
        /// Make the shipment an iParcel shipment
        /// </summary>
        public ShipmentEntityBuilder AsIParcel(Action<IParcelShipmentEntityBuilder> builderConfiguration) =>
            SetShipmentType(builderConfiguration, ShipmentTypeCode.iParcel, x => x.IParcel);

        /// <summary>
        /// Add a customs item to the shipment
        /// </summary>
        public ShipmentEntityBuilder WithCustomsItem() => WithCustomsItem(null);

        /// <summary>
        /// Add a customs item to the shipment
        /// </summary>
        public ShipmentEntityBuilder WithCustomsItem(Action<EntityBuilder<ShipmentCustomsItemEntity>> builderConfiguration)
        {
            EntityBuilder<ShipmentCustomsItemEntity> builder = new EntityBuilder<ShipmentCustomsItemEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.CustomsItems.Add(builder.Build()));

            return this;
        }

        /// <summary>
        /// Set the insurance policy
        /// </summary>
        public ShipmentEntityBuilder WithInsurancePolicy() => WithInsurancePolicy(null);

        /// <summary>
        /// Set the insurance policy
        /// </summary>
        public ShipmentEntityBuilder WithInsurancePolicy(Action<EntityBuilder<InsurancePolicyEntity>> builderConfiguration)
        {
            EntityBuilder<InsurancePolicyEntity> builder = new EntityBuilder<InsurancePolicyEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.InsurancePolicy, builder.Build());

            return this;
        }

        /// <summary>
        /// Set the shipment type
        /// </summary>
        private ShipmentEntityBuilder SetShipmentType<T, TBuilder>(Action<TBuilder> builderConfiguration,
            ShipmentTypeCode shipmentTypeCode,
            Expression<Func<ShipmentEntity, T>> shipmentAccessor)
            where T : EntityBase2, new()
            where TBuilder : EntityBuilder<T>, new()
        {
            TBuilder builder = new TBuilder();
            builderConfiguration?.Invoke(builder);

            Set(x => x.ShipmentTypeCode, shipmentTypeCode);
            Set(shipmentAccessor, builder.Build());

            return this;
        }
    }
}