using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Represents where a filter is going to be inserted \ moved \ copied to
    /// </summary>
    public class FilterLocation
    {
        FilterNodeEntity parentNode;
        int position;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterLocation(FilterNodeEntity parentNode, int position)
        {
            this.parentNode = parentNode;
            this.position = position;
        }

        /// <summary>
        /// The node that will be the parent
        /// </summary>
        public FilterNodeEntity ParentNode
        {
            get
            {
                return parentNode;
            }
            set
            {
                parentNode = value;
            }
        }

        /// <summary>
        /// The position among the parent's children
        /// </summary>
        public int Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
    }
}
