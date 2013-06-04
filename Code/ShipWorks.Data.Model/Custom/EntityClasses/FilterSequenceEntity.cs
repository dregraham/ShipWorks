using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extend functionality of the generated FilterSequenceEntity
    /// </summary>
    public partial class FilterSequenceEntity
    {
        /// <summary>
        /// The name of the filter the sequence is attached to.  Primarily used for sorting sequences.
        /// </summary>
        public string FilterName
        {
            get
            {
                if (Filter != null)
                {
                    return Filter.Name;
                }

                return "";
            }
        }
    }
}
