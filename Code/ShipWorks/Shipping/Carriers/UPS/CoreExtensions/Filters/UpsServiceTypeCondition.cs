using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.CoreExtensions;

namespace ShipWorks.Shipping.Carriers.UPS.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the UPS service type
    /// </summary>
    [ConditionElement("UPS Service", "Shipment.UPS.ServiceType")]
    public class UpsServiceTypeCondition : EnumCondition<UpsServiceType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsServiceTypeCondition()
        {
            Value = UpsServiceType.UpsGround;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string shipmentTypeCondition = ShippingFilterConditionUtility.GetUpsTypeCondition(context);

            using (SqlGenerationScope scope = context.PushScope(ShipmentFields.ShipmentID, UpsShipmentFields.ShipmentID, SqlGenerationScopeType.AnyChild))
            {
                return string.Format("(({0}) AND ({1}))",

                    // This makes sure its a fedex shipment type
                    shipmentTypeCondition,

                    // This actually deos the service test
                    scope.Adorn(GenerateSql(context.GetColumnReference(UpsShipmentFields.Service), context))

                    );
            }
        }
    }
}
