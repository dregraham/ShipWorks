using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Where the file comes from for a generic import that is coming from a file
    /// </summary>
    public enum GenericFileSourceTypeCode
    {
        Disk = 0,
        FTP = 1,
        Email = 2,
        Warehouse = 3
    }
}
