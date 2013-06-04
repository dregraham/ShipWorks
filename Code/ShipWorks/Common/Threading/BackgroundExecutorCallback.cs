using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Delegate used for the execution of a background operation
    /// </summary>
    public delegate void BackgroundExecutorCallback<T>(T item, object userState, BackgroundIssueAdder<T> issueAdder);
}
