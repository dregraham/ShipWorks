using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.CoreExtensions;

namespace ShipWorks.Shipping.Carriers.Postal.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the USPS postal confirmation type
    /// </summary>
    [ConditionElement("Postal Confirmation", "Shipment.Postal.ConfirmationType")]
    public class PostalConfirmationTypeCondition : EnumCondition<PostalConfirmationType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalConfirmationTypeCondition()
        {
            Value = PostalConfirmationType.Delivery;
            SelectedValues = new[] { Value };
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
                    scope.Adorn(GenerateSql(context.GetColumnReference(PostalShipmentFields.Confirmation), context))

                    );
            }
        }
    }
}
