using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for testing Checkout Status of an eBay order item.
    /// </summary>
    [ConditionElement("eBay Shipping Method", "EbayOrder.SelectedShippingMethod")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbaySelectedShippingMethodCondition : ValueChoiceCondition<EbayShippingMethod>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbaySelectedShippingMethodCondition()
        {
            Value = EbayShippingMethod.DirectToBuyer;
        }

        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
        public override ICollection<ValueChoice<EbayShippingMethod>> ValueChoices
        {
            get
            {
                // Exclude the DirectToBuyerOverridden value from the avaialble choices since it is 
                // the same thing as DirectToBuyer as far as the user is concerned
                return EnumHelper.GetEnumList<EbayShippingMethod>().Select(e => new ValueChoice<EbayShippingMethod>(e.Description, e.Value))
                                                                   .Where(m => m.Value != EbayShippingMethod.DirectToBuyerOverridden)
                                                                   .ToList();
            }
        }

        /// <summary>
        /// Generate the SQL for the filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // Since two of the enum values have the same meaning to the user, we're going to compare everything to GSP value
            // and just change the operator that is used in the filter depending on the selected value
            string formattedExpression = Value == EbayShippingMethod.GlobalShippingProgram ? "{2}({0} = {1})" : "{2}({0} != {1})";

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EbayOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                // Use the formatted expression to compare against the GSP value and negate the expression if necessary depending
                // on the Operator that the user defined for this condition/filter
                return scope.Adorn(string.Format(formattedExpression,
                                                    context.GetColumnReference(EbayOrderFields.SelectedShippingMethod), (int) EbayShippingMethod.GlobalShippingProgram,
                                                    (Operator == EnumEqualityOperator.Equals) ? "" : "NOT "));
            }
        }
    }
}
