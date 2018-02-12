using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Processed Status", "Shipment.Status")]
    public class ShipmentStatusCondition : ValueChoiceCondition<ShipmentStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentStatusCondition()
        {
            Value = ShipmentStatusType.None;
        }

        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
        public override ICollection<ValueChoice<ShipmentStatusType>> ValueChoices
        {
            get
            {
                return EnumHelper.GetEnumList<ShipmentStatusType>().Select(e => new ValueChoice<ShipmentStatusType>(e.Description, e.Value)).ToList();
            }
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            bool processed;
            bool voided;

            switch (Value)
            {
                case ShipmentStatusType.None:
                    processed = false;
                    voided = false;
                    break;

                case ShipmentStatusType.Processed:
                    processed = true;
                    voided = false;
                    break;

                case ShipmentStatusType.Voided:
                    processed = true;
                    voided = true;
                    break;

                default:
                    throw new InvalidOperationException("Invalid ShipmentStatusType value.");
            }

            return string.Format("{4}({0} = {1} AND {2} = {3})",
                context.GetColumnReference(ShipmentFields.Processed), processed ? "1" : "0",
                context.GetColumnReference(ShipmentFields.Voided), voided ? "1" : "0",
                (Operator == EqualityOperator.Equals) ? "" : "NOT ");
        }
    }
}
