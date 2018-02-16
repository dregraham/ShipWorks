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
    [ConditionElement("eBay Feedback", "EbayOrderItem.Feedback")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayFeedbackCondition : ValueChoiceCondition<EbayFeedbackConditionStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayFeedbackCondition()
        {
            Value = EbayFeedbackConditionStatusType.SellerNotLeftForBuyer;
        }

        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
        public override ICollection<ValueChoice<EbayFeedbackConditionStatusType>> ValueChoices
        {
            get
            {
                return EnumHelper.GetEnumList<EbayFeedbackConditionStatusType>().Select(e => new ValueChoice<EbayFeedbackConditionStatusType>(e.Description, e.Value)).ToList();
            }
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(ShipWorks.Filters.Content.SqlGeneration.SqlGenerationContext context)
        {
            // First we have to get from Order -> EbayOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, EbayOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                // generate the SQL for the condition
                string sql = "";

                switch (Value)
                {
                    case EbayFeedbackConditionStatusType.SellerLeftForBuyer:
                        sql = string.Format("{0} {1} {2}",
                            context.GetColumnReference(EbayOrderItemFields.FeedbackLeftType),
                            (Operator == EqualityOperator.Equals) ? "!=" : "=",
                            (int) EbayFeedbackType.None);
                        break;

                    case EbayFeedbackConditionStatusType.SellerNotLeftForBuyer:
                        sql = string.Format("{0} {1} {2}",
                            context.GetColumnReference(EbayOrderItemFields.FeedbackLeftType),
                            GetSqlOperator(),
                            (int) EbayFeedbackType.None);
                        break;

                    case EbayFeedbackConditionStatusType.BuyerLeftNegative:
                        sql = string.Format("{0} {1} {2}",
                            context.GetColumnReference(EbayOrderItemFields.FeedbackReceivedType),
                            GetSqlOperator(),
                            (int) EbayFeedbackType.Negative);
                        break;

                    case EbayFeedbackConditionStatusType.BuyerLeftNeutral:
                        sql = string.Format("{0} {1} {2}",
                            context.GetColumnReference(EbayOrderItemFields.FeedbackReceivedType),
                            GetSqlOperator(),
                            (int) EbayFeedbackType.Neutral);
                        break;

                    case EbayFeedbackConditionStatusType.BuyerLeftPositive:
                        sql = string.Format("{0} {1} {2}",
                            context.GetColumnReference(EbayOrderItemFields.FeedbackReceivedType),
                            GetSqlOperator(),
                            (int) EbayFeedbackType.Positive);
                        break;

                    case EbayFeedbackConditionStatusType.BuyerNotLeft:
                        sql = string.Format("{0} {1} {2}",
                            context.GetColumnReference(EbayOrderItemFields.FeedbackReceivedType),
                            GetSqlOperator(),
                            (int) EbayFeedbackType.None);
                        break;
                }


                return scope.Adorn(sql);
            }
        }
    }
}
