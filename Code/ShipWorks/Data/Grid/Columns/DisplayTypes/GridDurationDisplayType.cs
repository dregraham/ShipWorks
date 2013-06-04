using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// DisplayType for display durations
    /// </summary>
    public class GridDurationDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridDurationDisplayType()
        {
            PreviewInputType = GridColumnPreviewInputType.Value;
        }

        /// <summary>
        /// Turn the value into something more tangible
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            object value = base.GetEntityValue(entity);

            if (value == null)
            {
                return null;
            }

            return TimeSpan.FromSeconds((int) value);
        }

        /// <summary>
        /// Get the display text for the given value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            TimeSpan duration = (TimeSpan) value;

            string display = string.Format("{0:0}:{1:00}", duration.Minutes, duration.Seconds);

            if (duration.Hours > 0)
            {
                display = string.Format("{0}h {1}", duration.Hours, display);
            }

            if (duration.Days > 0)
            {
                display = string.Format("{0}d {1}", duration.Days, display);
            }

            return display;
        }

        /// <summary>
        /// Most durations fit in a smaller area
        /// </summary>
        public override int DefaultWidth
        {
            get
            {
                return 60;
            }
        }
    }
}
