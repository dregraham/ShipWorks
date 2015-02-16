using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.CoreExtensions
{
    /// <summary>
    /// Utility for helping with filter generation
    /// </summary>
    public static class ShippingFilterConditionUtility
    {
        /// <summary>
        /// Get a SQL block that restricts to a postal shipment type
        /// </summary>
        public static string GetPostalTypeCondition(SqlGenerationContext context)
        {
            return string.Format("{0} IN ({1}, {2}, {3}, {4}, {5})",
                context.GetColumnReference(ShipmentFields.ShipmentType),
                (int) ShipmentTypeCode.Endicia,
                (int) ShipmentTypeCode.Express1Endicia,
                (int) ShipmentTypeCode.Express1Stamps,
                (int) ShipmentTypeCode.PostalWebTools,
                (int) ShipmentTypeCode.Usps);
        }

        /// <summary>
        /// Get a SQL block that restricts to a fedex shipment type
        /// </summary>
        public static string GetFedExTypeCondition(SqlGenerationContext context)
        {
            return string.Format("{0} = {1}",
                context.GetColumnReference(ShipmentFields.ShipmentType),
                (int) ShipmentTypeCode.FedEx);
        }

        /// <summary>
        /// Get a SQL block that restricts to a ups shipment type
        /// </summary>
        public static string GetUpsTypeCondition(SqlGenerationContext context)
        {
            return string.Format("{0} IN ({1}, {2})",
                context.GetColumnReference(ShipmentFields.ShipmentType),
                (int) ShipmentTypeCode.UpsOnLineTools,
                (int) ShipmentTypeCode.UpsWorldShip);
        }
    }
}
