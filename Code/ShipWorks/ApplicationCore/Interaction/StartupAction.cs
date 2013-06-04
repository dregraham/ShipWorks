using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// What to do when ShipWorks first starts up.
    /// </summary>
    public enum StartupAction
    {
        Default = 0,
        OpenDatabaseSetup = 1,
        ContinueDatabaseUpgrade = 2
    }
}
