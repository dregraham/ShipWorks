using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Grid.Columns.SortProviders
{
    /// <summary>
    /// Provide sorting for the ActualThermalLabel field
    /// </summary>
    /// <remarks>
    /// We can't just use the normal enum sort because when a shipment is processed, null means Standard.
    /// When a shipment isn't yet processed, null just means 'not set'.
    /// </remarks>
    public class GridActualLabelFormatSortProvider : GridColumnEnumDescriptionSortProvider<ThermalLanguage> 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridActualLabelFormatSortProvider() : 
            base(ShipmentFields.ActualLabelFormat)
        {

        }

        /// <summary>
        /// Build the DbFunctionCall that will be used as the sort expression
        /// </summary>
        protected override DbFunctionCall BuildDbFunctionCall(EntityField2 sortField, StringBuilder builder)
        {
            // Before the main case statement, we need to see if we need to translate a NULL into a Standard value
            string translation = string.Format("IsNull({{0}}, CASE WHEN {{1}} = 1 AND {{2}} <> 1 THEN {0} ELSE NULL END)", (int) ThermalLanguage.None);
            string sortExpression = string.Format(builder.ToString(), translation);

            return new DbFunctionCall(sortExpression, new[] { sortField.Clone(), ShipmentFields.Processed, ShipmentFields.Voided });
        }
    }
}