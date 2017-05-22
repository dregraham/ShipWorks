using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Actions
{
    /// <summary>
    /// An enumeration for specifying the type of action queue entry.
    /// </summary>
    public enum ActionQueueType
    {
        UserInterface = 0,

        Scheduled = 1,

        DefaultPrint = 2
    }
}
