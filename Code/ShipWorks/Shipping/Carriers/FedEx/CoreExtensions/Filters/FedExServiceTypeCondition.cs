using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.CoreExtensions;

namespace ShipWorks.Shipping.Carriers.FedEx.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the FedEx service type
    /// </summary>
    [ConditionElement("FedEx Service", "Shipment.FedEx.ServiceType")]
    public class FedExServiceTypeCondition : EnumCondition<FedExServiceType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExServiceTypeCondition()
        {
            Value = FedExServiceType.FedExGround;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string shipmentTypeCondition = ShippingFilterConditionUtility.GetFedExTypeCondition(context);

            using (SqlGenerationScope scope = context.PushScope(ShipmentFields.ShipmentID, FedExShipmentFields.ShipmentID, SqlGenerationScopeType.AnyChild))
            {
                return string.Format("(({0}) AND ({1}))",

                    // This makes sure its a fedex shipment type
                    shipmentTypeCondition,

                    // This actually does the service test
                    scope.Adorn(GenerateSql(context.GetColumnReference(FedExShipmentFields.Service), context))

                    );
            }
        }
    }
}
