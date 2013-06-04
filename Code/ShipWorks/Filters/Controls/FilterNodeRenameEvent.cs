using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// delegate signature for the rename event
    /// </summary>
    public delegate void FilterNodeRenameEventHandler(object sender, FilterNodeRenameEventArgs e);

    /// <summary>
    /// EventArgs for when an object is being renamed
    /// </summary>
    public class FilterNodeRenameEventArgs : CancelEventArgs
    {
        // The object that is being renamed
        FilterNodeEntity filterNode;

        // The proposed name
        string proposed;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterNodeRenameEventArgs(FilterNodeEntity filterNode)
            : this(filterNode, null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterNodeRenameEventArgs(FilterNodeEntity filterNode, string proposed)
        {
            this.filterNode = filterNode;
            this.proposed = proposed;
        }

        /// <summary>
        /// The object that is being renamed
        /// </summary>
        public FilterNodeEntity FilterNode
        {
            get { return filterNode; }
        }

        /// <summary>
        /// The proposed value that the node will be renamed to
        /// </summary>
        public string Proposed
        {
            get
            {
                return proposed;
            }
            set
            {
                proposed = value;
            }
        }
    }
}
