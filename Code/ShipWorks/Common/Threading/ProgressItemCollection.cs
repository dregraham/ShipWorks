using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.UI;
using Interapptive.Shared.Collections;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Thread-safe collection of progress items with change notification
    /// </summary>
    public class ProgressItemCollection : ObservableCollection<ProgressItem>
    {
        /// <summary>
        /// Creates and adds a new ProgressItem of the given name
        /// </summary>
        public ProgressItem Add(string name)
        {
            ProgressItem item = new ProgressItem(name);
            Add(item);

            return item;
        }
    }
}
