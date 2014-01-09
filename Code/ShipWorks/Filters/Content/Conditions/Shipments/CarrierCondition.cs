using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
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
                return ShipmentTypeManager.ShipmentTypes
                    .Where(t => t.ShipmentTypeCode != ShipmentTypeCode.BestRate)
                    .Select(t => new ValueChoice<ShipmentTypeCode>(t.ShipmentTypeName, t.ShipmentTypeCode))
                    .ToArray();
            }
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipmentType), context);
        }
    }
}
