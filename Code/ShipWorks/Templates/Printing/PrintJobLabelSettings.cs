using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Templates.Media;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Holds properties that will be used in printing labels for a print job
    /// </summary>
    public class PrintJobLabelSettings
    {
        long labelSheetID;
        LabelPosition labelPosition = new LabelPosition();

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintJobLabelSettings(long labelSheetID)
        {
            this.labelSheetID = labelSheetID;
        }

        /// <summary>
        /// The label sheet that will be used
        /// </summary>
        public long LabelSheetID
        {
            get { return labelSheetID; }
            set { labelSheetID = value; }
        }

        /// <summary>
        /// The row that the first label will be in
        /// </summary>
        public LabelPosition LabelPosition
        {
            get { return labelPosition; }
            set { labelPosition = value; }
        }
    }
}
