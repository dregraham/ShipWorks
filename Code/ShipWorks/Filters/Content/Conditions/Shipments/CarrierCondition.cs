using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Provider", "Shipment.ShipmentType")]
    public class CarrierCondition : ValueChoiceCondition<ShipmentTypeCode>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierCondition()
        {
            Value = ShipmentTypeCode.None;
        }

        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
        public override ICollection<ValueChoice<ShipmentTypeCode>> ValueChoices
        {
            get
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    IShipmentTypeManager shipmentTypeManager = scope.Resolve<IShipmentTypeManager>();
                    IShippingManager shippingManager = scope.Resolve<IShippingManager>();
                    return shipmentTypeManager.ShipmentTypes
                        .Where(t => t.ShipmentTypeCode != ShipmentTypeCode.BestRate)
                        .Where(
                            t =>
                                t.ShipmentTypeCode != ShipmentTypeCode.Amazon ||
                                shippingManager.IsShipmentTypeConfigured(t.ShipmentTypeCode))
                        .Select(t => new ValueChoice<ShipmentTypeCode>(t.ShipmentTypeName, t.ShipmentTypeCode))
                        .ToArray();
                }
            }
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) =>
            GenerateSql(context.GetColumnReference(ShipmentFields.ShipmentType), context);
    }
}