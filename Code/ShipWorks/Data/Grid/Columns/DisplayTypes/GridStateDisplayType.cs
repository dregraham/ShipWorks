using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Grid column type for displaying states
    /// </summary>
    public class GridStateDisplayType : GridAbbreviationDisplayType
    {
        string prefix;

        /// <summary>
        /// Specify the entity field prefix to upll StateProvCode and CountryCode with
        /// </summary>
        /// <param name="prefix"></param>
        public GridStateDisplayType(string prefix)
        {
            this.prefix = prefix;
        }

        /// <summary>
        /// Gets the display text
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            EntityBase2 entity = value as EntityBase2;

            if (entity != null)
            {
                if (prefix == null)
                {
                    throw new InvalidOperationException("prefix cannot be null");
                }

                IEntityField2 field = entity.Fields[prefix + "StateProvCode"];
                string stateCode = (field == null) ? "" : (string)field.CurrentValue;

                field = entity.Fields[prefix + "CountryCode"];
                string countryCode = (field == null) ? "" : (string)field.CurrentValue;

                return FormatAbbreviation(stateCode, countryCode);
            }
            else
            {
                return base.GetDisplayText(value);
            }
        }

        /// <summary>
        /// Create the editor used to edit the settings
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridStateDisplayEditor(this);
        }

        /// <summary>
        /// Get the full name based on the given abbreviation
        /// </summary>
        protected override string GetFullName(string abbreviation, object customData)
        {
            return Geography.GetStateProvName(abbreviation, customData as string);
        }
    }
}
