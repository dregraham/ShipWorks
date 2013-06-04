﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Label Format", "Shipment.LabelFormat")]
    public class LabelFormatCondition : EnumCondition<LabelFormatType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelFormatCondition()
        {
            Value = LabelFormatType.Standard;
        }

        /// <summary>
        /// Generate the sql
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
            string thermalColumn = context.GetColumnReference(ShipmentFields.ThermalType);

            switch (format)
            {
                case LabelFormatType.Standard:
                    return string.Format("{0} IS NULL", thermalColumn);

                case LabelFormatType.Thermal:
                    return string.Format("{0} IS NOT NULL", thermalColumn);

                case LabelFormatType.EPL:
                    return string.Format("{0} = {1}", thermalColumn, (int) ThermalLabelType.EPL);

                case LabelFormatType.ZPL:
                    return string.Format("{0} = {1}", thermalColumn, (int) ThermalLabelType.ZPL);
            }

            return "";
        }
    }
}
