﻿using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.CoreExtensions;

namespace ShipWorks.Shipping.Carriers.Postal.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the USPS postal service type
    /// </summary>
    [ConditionElement("Postal Service", "Shipment.Postal.ServiceType")]
    public class PostalServiceTypeCondition : EnumCondition<PostalServiceType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalServiceTypeCondition()
        {
            Value = PostalServiceType.PriorityMail;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string shipmentTypeCondition = ShippingFilterConditionUtility.GetPostalTypeCondition(context);

            using (SqlGenerationScope scope = context.PushScope(ShipmentFields.ShipmentID, PostalShipmentFields.ShipmentID, SqlGenerationScopeType.AnyChild))
            {
                return string.Format("(({0}) AND ({1}))",

                    // This makes sure its a postal shipment type
                    shipmentTypeCondition,

                    // This actually deos the service test
                    scope.Adorn(GenerateSql(context.GetColumnReference(PostalShipmentFields.Service), context))

                    );
            }
        }
    }
}
