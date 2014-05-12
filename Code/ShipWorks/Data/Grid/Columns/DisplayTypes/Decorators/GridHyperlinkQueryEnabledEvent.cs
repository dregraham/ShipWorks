using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators
{
    /// <summary>
    /// Delegate event handler for querying if the hyperlink should be enabled
    /// </summary>
    public delegate void GridHyperlinkQueryEnabledEventHandler(object sender, GridHyperlinkQueryEnabledEventArgs e);

    /// <summary>
    /// Event raised to allow users of the hyperlink decorator to control when it should be displayed as a link
    /// </summary>
    public class GridHyperlinkQueryEnabledEventArgs : EventArgs
    {
        public EntityBase2 Entity { get; set; }
        bool enabled = true;
        object value;

        /// <summary>
        /// Construtor
        /// </summary>
        public GridHyperlinkQueryEnabledEventArgs(object value, EntityBase2 entity)
        {
            Entity = entity;
            this.value = value;
        }

        /// <summary>
        /// The processed value (as returned from ProcessRawValue)
        /// </summary>
        public object Value
        {
            get { return value; }
        }

        /// <summary>
        /// Can be set to false to disable the column being displayed as a hyperlink
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
    }
}
