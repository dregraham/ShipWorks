using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using Autofac.Features.OwnedInstances;
using Autofac;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Provider", "Shipment.ShipmentType")]
    public class CarrierCondition : ValueChoiceCondition<ShipmentTypeCode>
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>This constructor should be removed when we start using IoC more fully for filters</remarks>
        public CarrierCondition() : 
            this(IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IShipmentTypeManager>>().Value, new ShippingManagerWrapper())
        {
            Value = ShipmentTypeCode.None;
        }

        public CarrierCondition(IShipmentTypeManager shipmentTypeManager, IShippingManager shippingManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
        public override ICollection<ValueChoice<ShipmentTypeCode>> ValueChoices =>
            shipmentTypeManager.ShipmentTypeCodes
                .Where(t => t != ShipmentTypeCode.BestRate)
                .Where(t => t != ShipmentTypeCode.Amazon || shippingManager.IsShipmentTypeConfigured(t))
                .Select(t => new ValueChoice<ShipmentTypeCode>(EnumHelper.GetDescription(t), t))
                .ToArray();

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) =>
            GenerateSql(context.GetColumnReference(ShipmentFields.ShipmentType), context);
    }
}