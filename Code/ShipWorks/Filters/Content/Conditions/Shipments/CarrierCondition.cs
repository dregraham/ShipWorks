using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping;

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
        public CarrierCondition() : this(new ShipmentTypeManagerWrapper(), new ShippingManagerWrapper())
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
            shipmentTypeManager.ShipmentTypes
                .Where(t => t.ShipmentTypeCode != ShipmentTypeCode.BestRate)
                .Where(t => t.ShipmentTypeCode != ShipmentTypeCode.Amazon || shippingManager.IsShipmentTypeConfigured(t.ShipmentTypeCode))
                .Select(t => new ValueChoice<ShipmentTypeCode>(t.ShipmentTypeName, t.ShipmentTypeCode))
                .ToArray();

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) =>
            GenerateSql(context.GetColumnReference(ShipmentFields.ShipmentType), context);
    }
}