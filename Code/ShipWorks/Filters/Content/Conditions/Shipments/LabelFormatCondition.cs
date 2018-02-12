using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Actual Label Format", "Shipment.LabelFormat")]
    public class LabelFormatCondition : ValueChoiceCondition<LabelFormatType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelFormatCondition()
        {
            Value = LabelFormatType.Standard;
        }

        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
        public override ICollection<ValueChoice<LabelFormatType>> ValueChoices
        {
            get
            {
                return EnumHelper.GetEnumList<LabelFormatType>().Select(e => new ValueChoice<LabelFormatType>(e.Description, e.Value)).ToList();
            }
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            List<LabelFormatType> effectiveFormats = new List<LabelFormatType>();

            if (Operator == EqualityOperator.Equals)
            {
                effectiveFormats.Add(Value);
            }
            else
            {
                switch (Value)
                {
                    case LabelFormatType.Standard:
                        effectiveFormats.Add(LabelFormatType.Thermal);
                        break;

                    case LabelFormatType.Thermal:
                        effectiveFormats.Add(LabelFormatType.Standard);
                        break;

                    case LabelFormatType.EPL:
                        effectiveFormats.Add(LabelFormatType.ZPL);
                        effectiveFormats.Add(LabelFormatType.Standard);
                        break;

                    case LabelFormatType.ZPL:
                        effectiveFormats.Add(LabelFormatType.EPL);
                        effectiveFormats.Add(LabelFormatType.Standard);
                        break;
                }
            }

            StringBuilder sql = new StringBuilder();

            foreach (LabelFormatType format in effectiveFormats)
            {
                if (sql.Length > 0)
                {
                    sql.Append(" OR ");
                }

                sql.AppendFormat("({0})", GetFormatSql(context, format));
            }

            return sql.ToString();
        }

        /// <summary>
        /// Get the SQL to use for the given label format
        /// </summary>
        private string GetFormatSql(SqlGenerationContext context, LabelFormatType format)
        {
            string thermalColumn = context.GetColumnReference(FilterField);

            switch (format)
            {
                case LabelFormatType.Standard:
                    return string.Format("{0} IS NULL OR {0} = {1}", thermalColumn, (int)ThermalLanguage.None);

                case LabelFormatType.Thermal:
                    return string.Format("{0} IS NOT NULL AND {0} <> {1}", thermalColumn, (int)ThermalLanguage.None);

                case LabelFormatType.EPL:
                    return string.Format("{0} = {1}", thermalColumn, (int) ThermalLanguage.EPL);

                case LabelFormatType.ZPL:
                    return string.Format("{0} = {1}", thermalColumn, (int) ThermalLanguage.ZPL);
            }

            return "";
        }

        /// <summary>
        /// Field that will be used for filtering
        /// </summary>
        protected virtual EntityField2 FilterField { get; } = ShipmentFields.ActualLabelFormat;
    }
}
