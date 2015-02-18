using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Grid;
using ShipWorks.Stores.Platforms.Groupon;
using log4net;
using ShipWorks.Data;

namespace ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Grid
{
    /// <summary>
    /// column definition for displaying the Groupon orderID 
    /// </summary>
    public class GridGrouponOrderDisplayType : GridOrderNumberDisplayType
    {
        private static ILog log = LogManager.GetLogger(typeof(GridGrouponOrderDisplayType));

        /// <summary>
        /// Constructor
        /// </summary>
        public GridGrouponOrderDisplayType()
        {

        }
    }
}
