using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using System.IO;
using ShipWorks.ApplicationCore;
using System.Xml.Linq;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// The type of template import that is pending.
    /// </summary>
    public enum ShipWorks2xApplicationDataSourceType
    {
        AppDataFolder = 0,
        BackupFile = 1
    }
}
