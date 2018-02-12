using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Filter for ShipSenseOrderRecognitionStatus
    /// </summary>
    [ConditionElement("ShipSense", "Order.ShipSenseRecognitionStatus")]
    public class OrderShipSenseStatusCondition : EnumCondition<ShipSenseOrderRecognitionStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderShipSenseStatusCondition"/> class.
        /// </summary>
        public OrderShipSenseStatusCondition()
        {
            Value = ShipSenseOrderRecognitionStatus.Recognized;
            SelectedValues = new[] { Value };
        }

        /// <summary>
        /// Generate the SQL for the condition clement
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context required");
            }

            return base.GenerateSql(context.GetColumnReference(OrderFields.ShipSenseRecognitionStatus), context);
        }
    }
}
