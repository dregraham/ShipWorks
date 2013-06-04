using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.PaymentDetails
{
    [ConditionElement("Label", "PaymentDetail.Label")]
    public class PaymentDetailLabelCondition : StringCondition
    {
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderPaymentDetailFields.Label), context);
        }
    }
}
