using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Drawing;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Grid column type for displaying country values
    /// </summary>
    public class GridCountryDisplayType : GridAbbreviationDisplayType
    {
        bool showFlag = false;

        /// <summary>
        /// Create the editor used to edit the settings
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridCountryDisplayEditor(this);
        }

        /// <summary>
        /// Indicates if the countries flag should be drawn
        /// </summary>
        public bool ShowFlag
        {
            get { return showFlag; }
            set { showFlag = value; }
        }

        /// <summary>
        /// Get the ful name to use for the given abbreviation
        /// </summary>
        protected override string GetFullName(string abbreviation, object customData)
        {
            return Geography.GetCountryName(abbreviation);
        }

        /// <summary>
        /// Get the image to use for display
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            if (showFlag)
            {
                return Geography.GetCountryFlag(value.ToString());
            }
            else
            {
                return null;
            }
        }
    }
}
