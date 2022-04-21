using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.CoreExtensions;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the DhlEcommerce service type
    /// </summary>
    [ConditionElement("DHL eCommerce Service", "Shipment.DhlEcommerce.ServiceType")]
    public class DhlEcommerceServiceTypeCondition : EnumCondition<DhlEcommerceServiceType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceServiceTypeCondition()
        {
            Value = DhlEcommerceServiceType.US_DhlSmartMailParcelGround;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string shipmentTypeCondition = ShippingFilterConditionUtility.GetDhlEcommerceTypeCondition(context);

            using (SqlGenerationScope scope = context.PushScope(ShipmentFields.ShipmentID, DhlEcommerceShipmentFields.ShipmentID, SqlGenerationScopeType.AnyChild))
            {
                return string.Format("(({0}) AND ({1}))",

                    // This makes sure its a DhlEcommerce shipment type
                    shipmentTypeCondition,

                    // This actually does the service test
                    scope.Adorn(GenerateSql(context.GetColumnReference(DhlEcommerceShipmentFields.Service), context))

                    );
            }
        }
    }
}
