using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Data.Controls
{
    /// <summary>
    /// Enumerates the fields that are available for editing in the person control, so that they may be turned on and off.
    /// </summary>
    [Flags]
    [Editor(typeof(EnumFlagsTypeEditor), typeof(UITypeEditor))]
    public enum PersonFields
    {
        None        = 0x0000,
        Name        = 0x0001,
        Company     = 0x0002,
        Street      = 0x0004,
        City        = 0x0008,
        State       = 0x0010,
        Postal      = 0x0020,
        Country     = 0x0040,
        Residential = 0x0080,
        Email       = 0x0100,
        Phone       = 0x0200,
        Fax         = 0x0400,
        Website     = 0x0800,
        All         = 0x0FFF
    }
}
