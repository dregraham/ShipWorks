using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Insured By", "Shipment.Insurance")]
    public class ShipmentInsuredCondition : EnumCondition<ShipmentInsuredType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentInsuredCondition()
        {
            Value = ShipmentInsuredType.None;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (Value == ShipmentInsuredType.None)
            {
                return string.Format("{2}({0} = {1})",
                    context.GetColumnReference(ShipmentFields.Insurance), 0,
                    (Operator == EqualityOperator.Equals) ? "" : "NOT ");
            }
            else
            {
                return string.Format("{4}({0} = {1} AND {2} = {3})",
                    context.GetColumnReference(ShipmentFields.Insurance), "1",
                    context.GetColumnReference(ShipmentFields.InsuranceProvider), (Value == ShipmentInsuredType.ShipWorks) ? (int) InsuranceProvider.ShipWorks : (int) (InsuranceProvider.Carrier),
                    (Operator == EqualityOperator.Equals) ? "" : "NOT ");
            }
        }
    }
}
