using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.CoreExtensions;

namespace ShipWorks.Shipping.Carriers.UPS.CoreExtensions.Filters
{
    [ConditionElement("UPS Return Service", "Shipment.UPS.ReturnServiceType")]
    public class UpsReturnServiceCondition : EnumCondition<UpsReturnServiceType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsReturnServiceCondition()
        {
            Value = UpsReturnServiceType.PrintReturnLabel;
            SelectedValues = new[] { Value };
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string shipmentTypeCondition = ShippingFilterConditionUtility.GetUpsTypeCondition(context);
            string returnShipmentCondition =  string.Format("{0} = 1", context.GetColumnReference(ShipmentFields.ReturnShipment));

            using (SqlGenerationScope scope = context.PushScope(ShipmentFields.ShipmentID, UpsShipmentFields.ShipmentID, SqlGenerationScopeType.AnyChild))
            {
                return string.Format("(({0}) AND ({1}) AND ({2}))",

                    // This makes sure its a fedex shipment type
                    shipmentTypeCondition,

                    // only return shipments
                    returnShipmentCondition,

                    // This actually deos the return service test
                    scope.Adorn(GenerateSql(context.GetColumnReference(UpsShipmentFields.ReturnService), context))
                    );
            }
        }
    }
}
