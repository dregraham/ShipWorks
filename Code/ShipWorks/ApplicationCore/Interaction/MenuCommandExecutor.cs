using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;
using System.ComponentModel;
using System.Diagnostics;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Delegate for the callback function to execute a menu command
    /// </summary>
    public delegate void MenuCommandExecutor(MenuCommandExecutionContext context);
}
