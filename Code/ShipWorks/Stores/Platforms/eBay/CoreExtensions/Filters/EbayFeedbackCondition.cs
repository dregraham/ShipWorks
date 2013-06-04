using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Filters
{
    [ConditionElement("eBay Feedback", "EbayOrderItem.Feedback")]
    [ConditionStoreType(StoreTypeCode.Ebay)]
    public class EbayFeedbackCondition : EnumCondition<EbayFeedbackConditionStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayFeedbackCondition()
        {
            Value = EbayFeedbackConditionStatusType.SellerNotLeftForBuyer;
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
                            (Operator == EqualityOperator.Equals) ? "!=" : "=" ,
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
