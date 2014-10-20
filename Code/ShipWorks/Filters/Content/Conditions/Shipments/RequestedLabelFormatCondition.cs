using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    /// <summary>
    /// Filter on requested label format
    /// </summary>
    [ConditionElement("Requested Label Format", "Shipment.RequestedLabelFormat")]
    public class RequestedLabelFormatCondition : LabelFormatCondition
    {
        /// <summary>
        /// Field that will be used for filtering
        /// </summary>
        protected override EntityField2 FilterField
        {
            get
            {
                return ShipmentFields.RequestedLabelFormat;
            }
        }
    }
}