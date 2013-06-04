using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Function declaration for testing whether a grid column is applicable in a given situation.
    /// </summary>
    public delegate bool GridColumnApplicableTest(object contextData);
}
