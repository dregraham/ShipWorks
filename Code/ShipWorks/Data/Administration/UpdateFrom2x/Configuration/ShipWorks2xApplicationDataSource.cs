using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// Defines a source for where Application Data should be imported from ShipWorks2x
    /// </summary>
    public class ShipWorks2xApplicationDataSource
    {
        /// <summary>
        /// The type of object that Path refers to
        /// </summary>
        public ShipWorks2xApplicationDataSourceType SourceType
        {
            get;
            set;
        }

        /// <summary>
        /// The path to where templates could come from.  What the path refers to is determined by SourceType
        /// </summary>
        public string Path
        {
            get;
            set;
        }
    }
}
