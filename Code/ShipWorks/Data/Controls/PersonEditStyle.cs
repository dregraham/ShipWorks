using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Controls
{
    /// <summary>
    /// Ways the PersonControl can allow editing
    /// </summary>
    public enum PersonEditStyle
    {
        // The control is always in edit mode, the user can always type.
        Normal,

        // The control is not editable at all and is always readonly
        ReadOnly
    }
}
