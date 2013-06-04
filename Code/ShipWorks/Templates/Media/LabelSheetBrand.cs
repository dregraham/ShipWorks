using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Simple class representing a brand name and the sheets loaded for it.
    /// </summary>
    public class LabelSheetBrand
    {
        string name;

        // The sheets contained by this brand
        List<LabelSheetEntity> sheets = new List<LabelSheetEntity>();

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelSheetBrand(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// The brand name
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The sheets in the brand
        /// </summary>
        public List<LabelSheetEntity> Sheets
        {
            get { return sheets; }
        }
    }
}
